import { useEffect, useState, useRef } from "react";
import { useParams, Link, useNavigate } from "react-router-dom";
import { API, StaticAPI } from "../services/api";
import { Announcement } from "../types/ads";
import { useAuth } from "../context/AuthContext";

const AdPage = () => {
  const { announcementId } = useParams();
  const navigate = useNavigate();
  const { isAuth } = useAuth();

  const [ad, setAd] = useState<Announcement | null>(null);
  const [loading, setLoading] = useState(true);
  const [showMenu, setShowMenu] = useState(false);
  const [currentUserId, setCurrentUserId] = useState<string | null>(null);
  const [showConfirm, setShowConfirm] = useState(false);
  const [deleting, setDeleting] = useState(false);
  const [message, setMessage] = useState<string | null>(null);
  const [messageType, setMessageType] = useState<"success" | "error">("success");
  const menuRef = useRef<HTMLDivElement | null>(null);

  const isOwner = ad?.user != null && ad.user.userId === currentUserId;

  const showTempMessage = (msg: string, type: "success" | "error") => {
    setMessage(msg);
    setMessageType(type);
    setTimeout(() => setMessage(null), 4000);
  };

  const fetchAd = async () => {
    try {
      const response = await API.get(`/Announcement/${announcementId}`);
      setAd(response.data);
    } catch (error) {
      showTempMessage("Ошибка загрузки объявления", "error");
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    if (announcementId) fetchAd();
  }, [announcementId]);

  useEffect(() => {
    const fetchCurrentUserId = async () => {
      try {
        const res = await API.get("/user/me");
        setCurrentUserId(res.data.userId);
      } catch {
        // Игнорируем
      }
    };

    if (isAuth) {
      fetchCurrentUserId();
    }
  }, [isAuth]);

  useEffect(() => {
    const handleClickOutside = (event: MouseEvent) => {
      if (menuRef.current && !menuRef.current.contains(event.target as Node)) {
        setShowMenu(false);
      }
    };
    if (showMenu) {
      document.addEventListener("mousedown", handleClickOutside);
    }
    return () => {
      document.removeEventListener("mousedown", handleClickOutside);
    };
  }, [showMenu]);

  const handleAuthRedirect = (callback: () => void) => {
    if (!isAuth) {
      navigate("/login");
    } else {
      callback();
    }
  };

  const handleDelete = async () => {
    if (!ad?.announcementId) return;
    setDeleting(true);
    try {
      await API.delete("/announcement", {
        params: { announcementId: ad.announcementId },
      });
      showTempMessage("Объявление удалено", "success");
      setTimeout(() => navigate("/ads"), 800);
    } catch (err) {
      showTempMessage("Не удалось удалить объявление", "error");
    } finally {
      setDeleting(false);
    }
  };

  const handleBooking = async () => {
    try {
      await API.post("/booking/book", null, {
        params: { announcementId },
      });
      showTempMessage("Объявление забронировано", "success");
      fetchAd();
    } catch (err: any) {
      const msg = typeof err?.response?.data === "string"
      ? err.response.data
      : "Не удалось забронировать";
    showTempMessage(msg, "error");
    }
  };

  const handleUnbooking = async () => {
    try {
      await API.post("/booking/unbook", null, {
        params: { announcementId },
      });
      showTempMessage("Бронь отменена", "success");
      fetchAd();
    } catch (err: any) {
      const msg = err?.response?.data?.message || "Не удалось отменить бронь";
      showTempMessage(msg, "error");
    }
  };

  const handleCompleteTransaction = async () => {
    try {
      await API.post("/booking/complete", null, {
        params: { announcementId },
      });
      showTempMessage("Обмен завершён", "success");
      fetchAd(); // обновляем объявление
    } catch (err: any) {
      const msg = typeof err?.response?.data === "string"
        ? err.response.data
        : "Не удалось завершить обмен";
      showTempMessage(msg, "error");
    }
  };

  if (loading) return <div className="text-center py-8">Загрузка...</div>;
  if (!ad) return <div className="text-center py-8">Объявление не найдено</div>;

  return (
    <div className="max-w-7xl mx-auto py-8">
      {message && (
        <div className={`fixed top-4 left-1/2 transform -translate-x-1/2 px-6 py-3 rounded-xl shadow-md z-50 animate-fade-in ${
          messageType === "success" ? "bg-green-500 text-white" : "bg-red-500 text-white"
        }`}>
          {message}
        </div>
      )}

      {showConfirm && (
        <div className="fixed inset-0 z-50 flex items-center justify-center bg-black bg-opacity-50">
          <div className="bg-white rounded-xl p-6 w-full max-w-sm shadow-xl animate-fade-in">
            <h2 className="text-lg font-semibold mb-4">Удалить объявление?</h2>
            <p className="text-sm text-gray-600 mb-6">
              Вы точно хотите удалить это объявление? Это действие необратимо.
            </p>
            <div className="flex justify-end gap-3">
              <button
                className="px-4 py-2 rounded-md bg-gray-100 hover:bg-gray-200 text-sm"
                onClick={() => setShowConfirm(false)}
              >
                Отмена
              </button>
              <button
                className="px-4 py-2 rounded-md bg-red-600 hover:bg-red-700 text-white text-sm"
                onClick={handleDelete}
                disabled={deleting}
              >
                {deleting ? "Удаление..." : "Удалить"}
              </button>
            </div>
          </div>
        </div>
      )}

      <div className="bg-white rounded-xl shadow-lg p-6">
        <div className="flex justify-between items-start mb-6">
          <Link to="/ads" className="text-primary hover:text-primary-dark flex items-center gap-2">
            <svg className="w-4 h-4" fill="none" stroke="currentColor" viewBox="0 0 24 24">
              <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M10 19l-7-7m0 0l7-7m-7 7h18" />
            </svg>
            Назад к списку
          </Link>

          {isOwner && (
            <div className="relative" ref={menuRef}>
              <button
                onClick={() => setShowMenu(!showMenu)}
                className="hover:bg-gray-100 rounded-full p-2"
              >
                <svg className="w-5 h-5" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                  <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M12 5v.01M12 12v.01M12 19v.01" />
                </svg>
              </button>

              {showMenu && (
                <div className="absolute right-0 mt-2 w-48 bg-white rounded-xl shadow-lg py-1 border">
                  <button
                    onClick={() => navigate(`/ads/edit/${ad.announcementId}`)}
                    className="block w-full px-4 py-2 text-sm hover:bg-gray-100 text-left"
                  >
                    Редактировать
                  </button>
                  <button
                    onClick={() => setShowConfirm(true)}
                    className="block w-full px-4 py-2 text-sm hover:bg-gray-100 text-left text-red-600"
                  >
                    Удалить
                  </button>
                </div>
              )}
            </div>
          )}
        </div>

        {/* Content */}
        <div className="grid md:grid-cols-2 gap-8">
          <div className="relative h-96 rounded-xl overflow-hidden">
            <img
              src={`${StaticAPI.defaults.baseURL}${ad.image}`}
              alt={ad.title}
              className="w-full h-full object-cover"
              onError={(e) => {
                (e.target as HTMLImageElement).src = "/placeholder-image.jpg";
              }}
            />
          </div>

          <div className="space-y-6">
            <div>
              <h1 className="text-3xl font-bold mb-4">{ad.title}</h1>
              <div className="flex flex-wrap gap-4 mb-6">
                <span
                  className="px-3 py-1 rounded-full text-sm"
                  style={{ backgroundColor: ad.category?.color || "#4CAF50" }}
                >
                  {ad.category?.name}
                </span>
                <div className="flex items-center gap-2 text-gray-600">
                  <svg className="w-5 h-5" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                    <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M17.657 16.657L13.414 20.9a1.998 1.998 0 01-2.827 0l-4.244-4.243a8 8 0 1111.314 0z" />
                    <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M15 11a3 3 0 11-6 0 3 3 0 016 0z" />
                  </svg>
                  <span>{[ad.address?.city, ad.address?.street, ad.address?.house].filter(Boolean).join(", ")}</span>
                </div>
              </div>

              <div className="grid grid-cols-2 gap-4 bg-gray-50 p-4 rounded-xl mb-6">
                <div>
                  <p className="text-sm text-gray-500 mb-1">Срок годности</p>
                  <p className="font-medium">
                    {ad.expirationDate ? new Date(ad.expirationDate).toLocaleDateString("ru-RU") : "Не указано"}
                  </p>
                </div>
                <div>
                  <p className="text-sm text-gray-500 mb-1">Опубликовано</p>
                  <p className="font-medium">
                    {new Date(ad.dateCreation).toLocaleDateString("ru-RU")}
                  </p>
                </div>
              </div>

              <div className="mb-6">
                <h2 className="text-lg font-semibold mb-2">Описание</h2>
                <p className="text-gray-600 whitespace-pre-line">{ad.description}</p>
              </div>
            </div>

            <div className="border-t pt-6">
                <p className="text-sm text-gray-500 mb-4">Автор объявления</p>

                {/* Автор (пользователь) */}
                {ad.user && (
                  <Link
                    to={`/profile/user/${ad.user.userId}`}
                    className="flex items-center gap-4 mb-4 hover:bg-gray-50 p-2 rounded-lg transition"
                  >
                    <img
                      src={
                        ad.user.image
                          ? `${StaticAPI.defaults.baseURL}${ad.user.image}`
                          : "/default-avatar.png"
                      }
                      alt={ad.user.userName}
                      className="w-12 h-12 rounded-full object-cover"
                    />
                    <div>
                      <p className="font-medium">{ad.user.userName}</p>
                    </div>
                  </Link>
                )}

                {/* Организация */}
                {ad.organization && (
                  <>
                    <p className="text-sm text-gray-500 mb-2">Организация</p>
                    <Link
                      to={`/organizations/${ad.organization.id}`}
                      className="flex items-center gap-4 mb-6 hover:bg-gray-50 p-2 rounded-lg transition"
                    >
                      <img
                        src={
                          ad.organization.logoImage
                            ? `${StaticAPI.defaults.baseURL}${ad.organization.logoImage}`
                            : "/default-logo.png"
                        }
                        alt={ad.organization.name}
                        className="w-12 h-12 rounded-full object-cover"
                      />
                      <div>
                        <p className="font-medium">{ad.organization.name}</p>
                      </div>
                    </Link>
                  </>
                )}

                {/* Кнопки действий */}
                {!isOwner ? (
                  ad.status === "Забронировано" && !ad.isBookedByCurrentUser ? (
                    <div className="bg-gray-200 text-gray-700 py-3 px-6 rounded-xl text-center font-medium">
                      Забронировано
                    </div>
                  ) : (
                    <div className="flex gap-4">
                      {ad.isBookedByCurrentUser ? (
                        <button
                          className="flex-1 bg-red-600 hover:bg-red-700 text-white py-3 px-6 rounded-xl font-medium transition-colors"
                          onClick={() => handleAuthRedirect(handleUnbooking)}
                        >
                          Отменить бронь
                        </button>
                      ) : (
                        <button
                          className="flex-1 bg-primary hover:bg-green-600 text-white py-3 px-6 rounded-xl font-medium transition-colors"
                          onClick={() => handleAuthRedirect(handleBooking)}
                        >
                          Забронировать
                        </button>
                      )}
                      <button
                        className="flex-1 border-2 border-primary text-primary bg-white hover:bg-gray-50 py-3 px-6 rounded-xl font-medium"
                        onClick={() => handleAuthRedirect(() => console.log("Написать..."))}
                      >
                        Написать
                      </button>
                    </div>
                  )
                ) : (
                  <div className="space-y-3">
                    <div className="text-sm text-gray-600">
                      Статус: <span className="font-medium">{ad.status || "активно"}</span>
                    </div>

                    {ad.status === "Забронировано" && (
                      <button
                        className="w-full bg-amber-600 hover:bg-amber-700 text-white py-3 px-6 rounded-xl font-medium transition-colors"
                        onClick={handleCompleteTransaction}
                      >
                        Завершить обмен
                      </button>
                    )}
                  </div>
                )}
              </div>

          </div>
        </div>
      </div>
    </div>
  );
};

export default AdPage;

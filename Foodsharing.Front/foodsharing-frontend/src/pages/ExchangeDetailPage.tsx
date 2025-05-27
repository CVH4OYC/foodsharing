import { useEffect, useState } from "react";
import { useParams, Link, useNavigate } from "react-router-dom";
import { API, StaticAPI } from "../services/api";
import { TransactionDTO } from "../types/transaction";
import { useAuth } from "../context/AuthContext";

const ExchangeDetailPage = () => {
  const { transactionId } = useParams();
  const navigate = useNavigate();
  const { isAuth } = useAuth();

  const [transaction, setTransaction] = useState<TransactionDTO | null>(null);
  const [loading, setLoading] = useState(false);
  const [currentUserId, setCurrentUserId] = useState<string | null>(null);
  const [message, setMessage] = useState<string | null>(null);
  const [messageType, setMessageType] = useState<"success" | "error">("success");

  const fetchExchange = async (silent = false) => {
    if (!silent) setLoading(true);
    try {
      const res = await API.get(`/Transaction/${transactionId}`);
      setTransaction(res.data);
    } catch (err) {
      console.error("Ошибка загрузки обмена", err);
    } finally {
      if (!silent) setLoading(false);
    }
  };

  useEffect(() => {
    if (transactionId) fetchExchange();
  }, [transactionId]);

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

  const showTempMessage = (msg: string, type: "success" | "error") => {
    setMessage(msg);
    setMessageType(type);
    setTimeout(() => setMessage(null), 4000);
  };

  const handleAuthRedirect = (callback: () => void) => {
    if (!isAuth) {
      navigate("/login");
    } else {
      callback();
    }
  };

  const handleCancel = async () => {
    try {
      await API.post("/booking/unbook", null, {
        params: { announcementId: transaction?.announcement.announcementId },
      });
      showTempMessage("Бронь отменена", "success");
      await fetchExchange(true);
    } catch (err: any) {
      showTempMessage("Не удалось отменить бронь", "error");
    }
  };

  const handleComplete = async () => {
    try {
      await API.post("/booking/complete", null, {
        params: { announcementId: transaction?.announcement.announcementId },
      });
      showTempMessage("Обмен завершён", "success");
      await fetchExchange(true);
    } catch (err: any) {
      showTempMessage("Не удалось завершить обмен", "error");
    }
  };

  const handleOpenChat = async (userId: string) => {
    try {
      const res = await API.get(`/chat/with/${userId}`);
      navigate(`/chats/${res.data}`);
    } catch (err: any) {
      if (err?.response?.status === 404) {
        navigate(`/chats/new?userId=${userId}`);
      } else {
        showTempMessage("Не удалось открыть чат", "error");
      }
    }
  };

  // ✅ Меняем поведение загрузки
  if (!transaction && loading) return <div className="text-center py-8">Загрузка...</div>;
  if (!transaction) return <div className="text-center py-8">Обмен не найден</div>;

  const isSender = currentUserId === transaction.sender.userId;
  const isRecipient = currentUserId === transaction.recipient.userId;
  const canCancel = transaction.status === "Забронировано" && isRecipient;
  const canComplete = transaction.status === "Забронировано" && isSender;
  const companionId = isSender ? transaction.recipient.userId : transaction.sender.userId;

  return (
    <div className="max-w-7xl mx-auto py-8">
      {/* message-баннер */}
      {message && (
        <div className={`fixed top-4 left-1/2 transform -translate-x-1/2 px-6 py-3 rounded-xl shadow-md z-50 animate-fade-in ${
          messageType === "success" ? "bg-green-500 text-white" : "bg-red-500 text-white"
        }`}>
          {message}
        </div>
      )}

      <div className="bg-white rounded-xl shadow-lg p-6">
        <div className="flex justify-between items-start mb-6">
          <Link
            to="/profile/exchanges"
            className="text-primary hover:text-primary-dark flex items-center gap-2"
          >
            <svg className="w-4 h-4" fill="none" stroke="currentColor" viewBox="0 0 24 24">
              <path
                strokeLinecap="round"
                strokeLinejoin="round"
                strokeWidth={2}
                d="M10 19l-7-7m0 0l7-7m-7 7h18"
              />
            </svg>
            Назад к обменам
          </Link>
        </div>

        <div className="grid md:grid-cols-2 gap-8">
          <div className="relative h-96 rounded-xl overflow-hidden">
            {transaction.announcement?.image ? (
              <img
                src={`${StaticAPI.defaults.baseURL}${transaction.announcement.image}`}
                alt={transaction.announcement.title}
                className="w-full h-full object-cover"
              />
            ) : (
              <div className="w-full h-full bg-gray-200" />
            )}
          </div>

          <div className="space-y-6">
            <div>
              <h1 className="text-3xl font-bold mb-4">
                <Link
                  to={`/ads/${transaction.announcement.announcementId}`}
                  className="text-primary hover:underline"
                >
                  {transaction.announcement.title}
                </Link>
              </h1>

              <div className="grid grid-cols-2 gap-4 bg-gray-50 p-4 rounded-xl mb-6">
                <div>
                  <p className="text-sm text-gray-500 mb-1">Дата обмена</p>
                  <p className="font-medium">
                    {new Date(transaction.transactionDate).toLocaleDateString("ru-RU")}
                  </p>
                </div>
                <div>
                  <p className="text-sm text-gray-500 mb-1">Статус</p>
                  <p className="font-medium">{transaction.status}</p>
                </div>
              </div>

              <div className="space-y-6">
                <div>
                  <p className="text-sm text-gray-500 mb-2">Отдаёт еду</p>
                  {transaction.organization && (
                    <Link
                      to={`/organizations/${transaction.organization.id}`}
                      className="flex items-center gap-4 mb-4 hover:bg-gray-50 p-2 rounded-lg transition"
                    >
                      <img
                        src={
                          transaction.organization.logoImage
                            ? `${StaticAPI.defaults.baseURL}${transaction.organization.logoImage}`
                            : "/default-logo.png"
                        }
                        alt={transaction.organization.name}
                        className="w-12 h-12 rounded-full object-cover"
                      />
                      <div>
                        <p className="font-medium">{transaction.organization.name}</p>
                      </div>
                    </Link>
                  )}

                  <Link
                    to={`/profile/user/${transaction.sender.userId}`}
                    className="flex items-center gap-4 hover:bg-gray-50 p-2 rounded-lg transition"
                  >
                    <img
                      src={
                        transaction.sender.image
                          ? `${StaticAPI.defaults.baseURL}${transaction.sender.image}`
                          : `${StaticAPI.defaults.baseURL}/default-avatar.png`
                      }
                      alt={transaction.sender.firstName}
                      className="w-12 h-12 rounded-full object-cover"
                    />
                    <div>
                      <p className="font-medium">
                        {transaction.sender.firstName} {transaction.sender.lastName}
                      </p>
                      {transaction.organization && (
                        <p className="text-sm text-gray-500">Представитель</p>
                      )}
                    </div>
                  </Link>
                </div>

                <div>
                  <p className="text-sm text-gray-500 mb-2">Получает еду</p>
                  <Link
                    to={`/profile/user/${transaction.recipient.userId}`}
                    className="flex items-center gap-4 hover:bg-gray-50 p-2 rounded-lg transition"
                  >
                    <img
                      src={
                        transaction.recipient.image
                          ? `${StaticAPI.defaults.baseURL}${transaction.recipient.image}`
                          : `${StaticAPI.defaults.baseURL}/default-avatar.png`
                      }
                      alt={transaction.recipient.firstName}
                      className="w-12 h-12 rounded-full object-cover"
                    />
                    <div>
                      <p className="font-medium">
                        {transaction.recipient.firstName} {transaction.recipient.lastName}
                      </p>
                    </div>
                  </Link>
                </div>

                {(canCancel || canComplete || companionId) && (
                  <div className="pt-4 border-t flex flex-wrap gap-4">
                    {canCancel && (
                      <button
                        onClick={() => handleAuthRedirect(handleCancel)}
                        className="flex-1 basis-0 max-w-full bg-red-600 hover:bg-red-700 text-white py-3 px-6 rounded-xl font-medium transition-colors"
                      >
                        Отменить бронь
                      </button>
                    )}
                    {canComplete && (
                      <button
                        onClick={() => handleAuthRedirect(handleComplete)}
                        className="flex-1 basis-0 max-w-full bg-amber-600 hover:bg-amber-700 text-white py-3 px-6 rounded-xl font-medium transition-colors"
                      >
                        Завершить обмен
                      </button>
                    )}
                    {companionId && (
                      <button
                        onClick={() => handleAuthRedirect(() => handleOpenChat(companionId))}
                        className="flex-1 basis-0 max-w-full border-2 border-primary text-primary bg-white hover:bg-gray-50 py-3 px-6 rounded-xl font-medium"
                      >
                        Написать
                      </button>
                    )}
                  </div>
                )}

              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  );
};

export default ExchangeDetailPage;

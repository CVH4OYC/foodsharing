import { useEffect, useState } from "react";
import { useParams, useNavigate, Link } from "react-router-dom";
import { API, StaticAPI } from "../services/api";
import NotificationBanner from "../components/NotificationBanner";

const PartnershipApplicationDetailPage = () => {
  const { applicationId } = useParams();
  const navigate = useNavigate();

  const [application, setApplication] = useState<any>(null);
  const [loading, setLoading] = useState(true);
  const [message, setMessage] = useState<string | null>(null);
  const [messageType, setMessageType] = useState<"success" | "error">("success");
  const [showConfirm, setShowConfirm] = useState(false);
  const [comment, setComment] = useState("");
  const [confirmAction, setConfirmAction] = useState<"accept" | "reject" | null>(null);

  const showTempMessage = (msg: string, type: "success" | "error") => {
    setMessage(msg);
    setMessageType(type);
    setTimeout(() => setMessage(null), 4000);
  };

  const fetchApplication = async () => {
    try {
      const res = await API.get(`/parthnership/application/${applicationId}`);
      setApplication(res.data);
    } catch (err) {
      console.error("Ошибка при загрузке заявки", err);
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    fetchApplication();
  }, [applicationId]);

  const handleSubmitAction = async () => {
    try {
      const endpoint = confirmAction === "accept"
        ? "/parthnership/application/accept"
        : "/parthnership/application/reject";

      const res = await API.put(endpoint, {
        applicationId,
        comment: comment.trim() || undefined,
      });

      showTempMessage(res.data || (confirmAction === "accept" ? "Заявка принята" : "Заявка отклонена"), "success");

      setTimeout(() => {
        navigate(confirmAction === "accept"
          ? `/organizations/${application.organizationId}`
          : "/applications");
      }, 1200);
    } catch (err: any) {
      const errorText =
        typeof err?.response?.data === "string"
          ? err.response.data
          : err?.response?.data?.title || "Ошибка";
      showTempMessage(errorText, "error");
    } finally {
      setShowConfirm(false);
    }
  };

  if (loading) return <div className="p-4">Загрузка...</div>;
  if (!application) return <div className="p-4 text-red-600">Заявка не найдена</div>;

  const logoUrl = application.organization?.logoImage
    ? `${StaticAPI.defaults.baseURL}${application.organization.logoImage}`
    : "/default-logo.png";

  const isReviewed = application.status?.toLowerCase() === "рассмотрено";

  return (
    <div className="w-full mt-8 mb-16 px-0">
      {message && <NotificationBanner message={message} type={messageType} />}

      {showConfirm && (
        <div className="fixed inset-0 z-50 flex items-center justify-center bg-black bg-opacity-50">
          <div className="bg-white rounded-xl p-6 w-full max-w-sm shadow-xl animate-fade-in">
            <h2 className="text-lg font-semibold mb-4">
              {confirmAction === "accept" ? "Принять заявку?" : "Отклонить заявку?"}
            </h2>
            <textarea
              placeholder="Комментарий..."
              value={comment}
              onChange={(e) => setComment(e.target.value)}
              className="w-full border rounded-md p-2 mb-4 text-sm resize-none"
              rows={4}
            />
            <div className="flex justify-end gap-3">
              <button
                className="px-4 py-2 rounded-md bg-gray-100 hover:bg-gray-200 text-sm"
                onClick={() => setShowConfirm(false)}
              >
                Отмена
              </button>
              <button
                className={`px-4 py-2 rounded-md text-white text-sm ${confirmAction === "accept" ? "bg-green-600 hover:bg-green-700" : "bg-red-600 hover:bg-red-700"}`}
                onClick={handleSubmitAction}
              >
                {confirmAction === "accept" ? "Принять" : "Отклонить"}
              </button>
            </div>
          </div>
        </div>
      )}

      <div className="flex items-center justify-between flex-wrap gap-4 mb-6">
        <h1 className="text-2xl font-bold">
          Заявка от {application.organization?.name || "Неизвестно"}
        </h1>
        <img
          src={logoUrl}
          alt="Логотип организации"
          className="w-16 h-16 rounded-full object-cover border"
        />
      </div>

      <div className="space-y-4 text-sm">
        <div><strong>ID заявки:</strong> {application.id}</div>
        <div><strong>Дата подачи:</strong> {new Date(application.submittedAt).toLocaleString("ru-RU")}</div>
        <div><strong>Статус:</strong> {application.status}</div>

        <div className="mt-6">
          <h2 className="text-lg font-semibold mb-2">Организация</h2>
          <div><strong>Телефон:</strong> {application.organization?.phone}</div>
          <div><strong>Email:</strong> {application.organization?.email}</div>
          <div><strong>Адрес:</strong> {[application.organization?.address?.region, application.organization?.address?.city, application.organization?.address?.street, application.organization?.address?.house].filter(Boolean).join(", ")}</div>
          <div><strong>Форма:</strong> {application.organization?.organizationForm}</div>
          <div><strong>Сайт:</strong> {application.organization?.website || "—"}</div>
          <div><strong>Описание:</strong> {application.organization?.description || "—"}</div>
        </div>

        <div className="mt-6">
          <h2 className="text-lg font-semibold mb-2">Контактное лицо</h2>
          <div><strong>Телефон:</strong> {application.phone || "—"}</div>
          <div><strong>Email:</strong> {application.email || "—"}</div>
        </div>

        {!isReviewed && (
          <div className="mt-8 flex gap-4">
            <button
              className="bg-green-500 hover:bg-green-600 text-white px-4 py-2 rounded-xl transition"
              onClick={() => {
                setConfirmAction("accept");
                setShowConfirm(true);
              }}
            >
              Принять
            </button>
            <button
              className="bg-red-500 hover:bg-red-600 text-white px-4 py-2 rounded-xl transition"
              onClick={() => {
                setConfirmAction("reject");
                setShowConfirm(true);
              }}
            >
              Отклонить
            </button>
          </div>
        )}

        {isReviewed && (
            <div className="mt-8">
              <h2 className="text-lg font-semibold mb-2">Комментарий администратора</h2>
              <p>{application.comment || "—"}</p>

              {application.reviewedBy && (
                <div className="mt-4">
                  <h2 className="text-lg font-semibold mb-2">Рассмотрено администратором</h2>
                  <Link
                    to={`/profile/user/${application.reviewedBy.userId}`}
                    className="flex items-center gap-4 mb-4 hover:bg-gray-50 p-2 rounded-lg transition"
                  >
                    {application.reviewedBy.image ? (
                      <img
                        src={`${StaticAPI.defaults.baseURL}${application.reviewedBy.image}`}
                        alt={application.reviewedBy.userName}
                        className="w-12 h-12 rounded-full object-cover"
                      />
                    ) : (
                      <div className="w-12 h-12 rounded-full bg-gray-200 flex items-center justify-center">
                        <span className="font-medium">{application.reviewedBy.userName?.[0]?.toUpperCase() || "U"}</span>
                      </div>
                    )}
                    <div>
                      <p className="font-medium">{application.reviewedBy.userName}</p>
                      <p className="text-sm text-gray-500">
                        {application.reviewedAt && new Date(application.reviewedAt).toLocaleString("ru-RU")}
                      </p>
                    </div>
                  </Link>
                </div>
              )}
            </div>
          )}
      </div>
    </div>
  );
};

export default PartnershipApplicationDetailPage;
import { useEffect, useState } from "react";
import { useParams } from "react-router-dom";
import { API, StaticAPI } from "../services/api";

const PartnershipApplicationDetailPage = () => {
  const { applicationId } = useParams();
  const [application, setApplication] = useState<any>(null);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
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

    fetchApplication();
  }, [applicationId]);

  if (loading) return <div className="p-4">Загрузка...</div>;
  if (!application) return <div className="p-4 text-red-600">Заявка не найдена</div>;

  const logoUrl = application.organization?.logoImage
    ? `${StaticAPI.defaults.baseURL}${application.organization.logoImage}`
    : "/default-logo.png";

  const isReviewed = application.status?.toLowerCase() === "рассмотрено";

  return (
    <div className="w-full mt-8 mb-16">
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
        <div><strong>Комментарий модератора:</strong> {application.comment || "—"}</div>

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
            <button className="bg-green-500 hover:bg-green-600 text-white px-4 py-2 rounded-xl transition">
              Принять
            </button>
            <button className="bg-red-500 hover:bg-red-600 text-white px-4 py-2 rounded-xl transition">
              Отклонить
            </button>
          </div>
        )}
      </div>
    </div>
  );
};

export default PartnershipApplicationDetailPage;

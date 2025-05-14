// OrgInfoPage.tsx
import { useParams } from "react-router-dom";
import { useEffect, useState } from "react";
import { API, StaticAPI } from "../services/api";

const OrgInfoPage = () => {
  const { orgId } = useParams();
  const [organization, setOrganization] = useState<any>(null);

  useEffect(() => {
    if (orgId) {
      API.get(`/organization/${orgId}`).then(res => setOrganization(res.data));
    }
  }, [orgId]);

  if (!organization) return <div className="text-sm text-gray-500">Загрузка информации...</div>;

  const logoUrl = organization.logoImage
    ? `${StaticAPI.defaults.baseURL}${organization.logoImage}`
    : "/default-logo.png";

  return (
    <div className="space-y-3 text-sm">
      <img src={logoUrl} alt="Логотип" className="w-16 h-16 object-cover rounded-full border" />
      <div><strong>Название:</strong> {organization.name}</div>
      <div><strong>Телефон:</strong> {organization.phone}</div>
      <div><strong>Email:</strong> {organization.email}</div>
      <div><strong>Адрес:</strong> {[organization?.address?.region, organization?.address?.city, organization?.address?.street, organization?.address?.house].filter(Boolean).join(", ")}</div>
      <div><strong>Форма:</strong> {organization.organizationForm}</div>
      <div><strong>Сайт:</strong> {organization.website || "—"}</div>
      <div><strong>Описание:</strong> {organization.description || "—"}</div>
    </div>
  );
};

export default OrgInfoPage;
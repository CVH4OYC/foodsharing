import { useEffect, useState } from "react";
import { useParams, Link } from "react-router-dom";
import { API, StaticAPI } from "../services/api";
import { OrganizationDTO } from "../types/org";
import { useAuth } from "../context/AuthContext";
import { HiHeart, HiOutlineHeart } from "react-icons/hi";

const OrganizationProfilePage = () => {
  const { orgId } = useParams();
  const [org, setOrg] = useState<OrganizationDTO | null>(null);
  const [loading, setLoading] = useState(true);
  const { isAuth, logout, hasRole } = useAuth();

  useEffect(() => {
    const fetchOrg = async () => {
      if (!orgId) return;
      setLoading(true);
      try {
        const res = await API.get(`/organization/${orgId}`);
        setOrg(res.data);
      } catch (err) {
        console.error("Ошибка загрузки организации", err);
      } finally {
        setLoading(false);
      }
    };

    fetchOrg();
  }, [orgId]);

  const toggleFavorite = async () => {
    if (!org) return;
    try {
      if (org.isFavorite) {
        await API.delete("/organization/favorite", {
          params: { orgId: org.id },
          withCredentials: true,
        });
      } else {
        await API.post("/organization/favorite", null, {
          params: { orgId: org.id },
          withCredentials: true,
        });
      }
      setOrg((prev) => prev && { ...prev, isFavorite: !prev.isFavorite });
    } catch (err) {
      console.error("Ошибка обновления избранного:", err);
    }
  };

  return (
    <div className="max-w-7xl mx-auto px-4 py-8">
      {loading ? (
        <div className="text-gray-500">Загрузка...</div>
      ) : org ? (
        <>
          <div className="flex items-center gap-4 mb-8">
            <img
              src={
                org.logoImage
                  ? `${StaticAPI.defaults.baseURL}${org.logoImage}`
                  : "/default-logo.png"
              }
              alt="Логотип организации"
              className="w-16 h-16 rounded-full object-cover"
            />
            <div className="flex-1">
              <div className="flex items-center gap-2">
                <h1 className="text-xl font-bold">{org.name}</h1>
                <button
                  onClick={toggleFavorite}
                  className="flex items-center gap-1 text-sm text-red-600 transition duration-200"
                >
                  {org.isFavorite ? (
                    <HiHeart className="w-5 h-5 text-red-600 transition duration-200" />
                  ) : (
                    <>
                      <HiOutlineHeart className="w-5 h-5 text-red-600 transition duration-200" />
                      <span className="hover:underline">Добавить в избранное</span>
                    </>
                  )}
                </button>
              </div>
              {org.organizationForm && (
                <p className="text-gray-500">{org.organizationForm}</p>
              )}
              {org.website && (
                <a
                  href={org.website}
                  target="_blank"
                  rel="noopener noreferrer"
                  className="text-blue-600"
                >
                  {org.website}
                </a>
              )}

              {/* Кнопка перехода для администратора */}
              {hasRole("Admin") && (
                <Link
                  to={`/admin/organizations/${orgId}`}
                  className="inline-block mt-2 text-sm bg-gray-200 hover:bg-gray-300 text-gray-800 px-4 py-1 rounded-md transition"
                >
                  Открыть как админ
                </Link>
              )}
            </div>
          </div>

          <div className="mb-6">
            <p>
              <strong>Телефон:</strong> {org.phone || "—"}
            </p>
            <p>
              <strong>Email:</strong> {org.email || "—"}
            </p>
            <p>
              <strong>Адрес:</strong>{" "}
              {[
                org.address?.region,
                org.address?.city,
                org.address?.street,
                org.address?.house,
              ]
                .filter(Boolean)
                .join(", ") || "—"}
            </p>
            {org.description && (
              <p className="mt-2 text-gray-600 whitespace-pre-line">
                {org.description}
              </p>
            )}
          </div>

          <h2 className="text-lg font-semibold mb-4">Объявления организации</h2>
          <div className="text-gray-400 italic">
            Пока что список объявлений не реализован
          </div>
        </>
      ) : (
        <div className="text-red-500">Организация не найдена</div>
      )}
    </div>
  );
};

export default OrganizationProfilePage;

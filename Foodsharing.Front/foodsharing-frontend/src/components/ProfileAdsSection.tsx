import { useEffect, useState } from "react";
import { API, StaticAPI } from "../services/api";
import { Announcement } from "../types/ads";
import AdCard from "../components/AdCard";

interface Props {}

const ProfileAdsSection: React.FC<Props> = () => {
  const [statusFilter, setStatusFilter] = useState<"active" | "completed" | "booked">("active");
  const [ads, setAds] = useState<Announcement[]>([]);
  const [loading, setLoading] = useState(false);

  useEffect(() => {
    const fetchAds = async () => {
      setLoading(true);
      try {
        const res = await API.get("/announcement/my", {
            params: { statusFilter },
          });
        setAds(res.data);
      } catch (err) {
        console.error("Ошибка загрузки объявлений", err);
      } finally {
        setLoading(false);
      }
    };

    fetchAds();
  }, [statusFilter]);

  return (
    <>
      <div className="flex justify-between items-center mb-4">
        <h1 className="text-2xl font-bold">Мои объявления</h1>
        <div className="flex gap-2">
          <button
            className={`px-4 py-2 rounded-xl text-sm ${
              statusFilter === "active" ? "bg-primary text-white" : "bg-white border"
            }`}
            onClick={() => setStatusFilter("active")}
          >
            Активные
          </button>
          <button
            className={`px-4 py-2 rounded-xl text-sm ${
              statusFilter === "booked" ? "bg-primary text-white" : "bg-white border"
            }`}
            onClick={() => setStatusFilter("booked")}
          >
            Забронированные
          </button>
          <button
            className={`px-4 py-2 rounded-xl text-sm ${
              statusFilter === "completed" ? "bg-primary text-white" : "bg-white border"
            }`}
            onClick={() => setStatusFilter("completed")}
          >
            Завершенные
          </button>
        </div>
      </div>

      {loading ? (
        <div className="text-center text-gray-500">Загрузка...</div>
      ) : ads.length === 0 ? (
        <div className="text-center text-gray-500">Нет объявлений</div>
      ) : (
        <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 gap-4">
          {ads.map((ad) => {
            const owner = ad.user ?? ad.organization;
            const ownerLink = ad.user
              ? `/profile/user/${ad.user.userId}`
              : ad.organization
              ? `/organizations/${ad.organization.id}`
              : null;
            const ownerImage = ad.user?.image
              ? `${StaticAPI.defaults.baseURL}${ad.user.image}`
              : ad.organization?.logoImage
              ? `${StaticAPI.defaults.baseURL}${ad.organization.logoImage}`
              : "/default-avatar.png";
            const ownerName = ad.user
              ? `${ad.user.firstName || ""} ${ad.user.lastName || ""}`.trim()
              : ad.organization?.name ?? "Неизвестно";

            return (
              <AdCard
                key={ad.announcementId}
                {...ad}
                image={`${StaticAPI.defaults.baseURL}${ad.image}`}
                categoryColor={ad.category?.color || "#4CAF50"}
                date={new Date(ad.dateCreation).toLocaleDateString("ru-RU")}
                owner={{
                  name: ownerName,
                  image: ownerImage,
                  link: ownerLink,
                }}
              />
            );
          })}
        </div>
      )}
    </>
  );
};

export default ProfileAdsSection;

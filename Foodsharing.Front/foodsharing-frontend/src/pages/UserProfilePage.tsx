import { useParams } from "react-router-dom";
import { useEffect, useState } from "react";
import { API, StaticAPI } from "../services/api";
import { Announcement, UserProfile } from "../types/ads";
import AdCard from "../components/AdCard";

const UserProfilePage = () => {
  const { userId } = useParams();
  const [profile, setProfile] = useState<UserProfile | null>(null);
  const [ads, setAds] = useState<Announcement[]>([]);
  const [loading, setLoading] = useState(false);

  useEffect(() => {
    const fetchUserData = async () => {
      if (!userId) return;
      setLoading(true);
      try {
        const [profileRes, adsRes] = await Promise.all([
          API.get(`/user/${userId}`),
          API.get(`/announcement/user/${userId}`)
        ]);
        setProfile(profileRes.data);
        setAds(adsRes.data);
      } catch (err) {
        console.error("Ошибка загрузки данных пользователя", err);
      } finally {
        setLoading(false);
      }
    };

    fetchUserData();
  }, [userId]);

  return (
    <div className="max-w-7xl mx-auto px-4 py-8">
      {profile ? (
        <div className="flex items-center gap-4 mb-8">
          <img
            src={
              profile.image
                ? `${StaticAPI.defaults.baseURL}${profile.image}`
                : "/default-avatar.png"
            }
            alt={profile.userName}
            className="w-16 h-16 rounded-full object-cover"
          />
          <div>
            <h1 className="text-xl font-bold">
              {profile.firstName} {profile.lastName}
            </h1>
            <p className="text-gray-500">@{profile.userName}</p>
            {profile.bio && <p className="text-gray-600">{profile.bio}</p>}
          </div>
        </div>
      ) : (
        <div className="text-gray-500 mb-6">Загрузка профиля...</div>
      )}

      <h2 className="text-lg font-semibold mb-4">Активные объявления</h2>

      {loading ? (
        <div className="text-gray-500">Загрузка...</div>
      ) : ads.length === 0 ? (
        <div className="text-gray-500">У пользователя пока нет активных объявлений</div>
      ) : (
        <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 gap-4">
          {ads.map((ad) => (
            <AdCard
              key={ad.announcementId}
              {...ad}
              image={`${StaticAPI.defaults.baseURL}${ad.image}`}
              user={{
                ...ad.user,
                image: ad.user.image
                  ? `${StaticAPI.defaults.baseURL}${ad.user.image}`
                  : undefined,
              }}
              categoryColor={ad.category?.color || "#4CAF50"}
              date={new Date(ad.dateCreation).toLocaleDateString("ru-RU")}
            />
          ))}
        </div>
      )}
    </div>
  );
};

export default UserProfilePage;

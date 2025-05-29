import { useEffect, useState } from "react";
import { API, StaticAPI } from "../services/api";
import { UserProfile } from "../types/ads";
import { useNavigate } from "react-router-dom";

const ProfileInfoSection = () => {
  const [profile, setProfile] = useState<UserProfile | null>(null);
  const navigate = useNavigate();

  useEffect(() => {
    const fetchProfile = async () => {
      try {
        const res = await API.get("/user/my");
        setProfile(res.data);
      } catch (err) {
        console.error("Ошибка загрузки профиля", err);
      }
    };

    fetchProfile();
  }, []);

  if (!profile) return <div className="text-gray-500">Загрузка профиля...</div>;

  return (
    <div className="space-y-4">
      {/* Верхняя панель: инфа слева, кнопка справа */}
      <div className="flex items-start justify-between">
        <div className="flex items-center gap-4">
          <img
            src={
              profile.image
                ? `${StaticAPI.defaults.baseURL}${profile.image}`
                : `${StaticAPI.defaults.baseURL}/default-avatar.png`
            }
            alt={profile.userName}
            className="w-16 h-16 rounded-full object-cover"
          />
          <div>
            <h1 className="text-xl font-bold">
              {profile.firstName} {profile.lastName}
            </h1>
            <p className="text-gray-500">@{profile.userName}</p>
            {profile.rating != null ? (
              <p className="text-sm text-yellow-500">
                Рейтинг: {profile.rating.toFixed(1)} ★
              </p>
            ) : (
              <p className="text-sm text-gray-500">Пока нет отзывов</p>
            )}
          </div>
        </div>

        <button
          onClick={() => navigate("/profile/edit")}
          className="bg-primary hover:bg-green-600 text-white py-2 px-4 rounded-xl text-sm font-medium"
        >
          Редактировать
        </button>
      </div>

      {profile.bio && <p className="text-gray-600">{profile.bio}</p>}
    </div>
  );
};

export default ProfileInfoSection;

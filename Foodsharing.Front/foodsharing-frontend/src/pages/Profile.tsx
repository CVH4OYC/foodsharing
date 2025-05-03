import { useNavigate } from "react-router-dom";
import { API } from "../services/api";
import { useAuth } from "../context/AuthContext";

const Profile = () => {
  const navigate = useNavigate();
  const { logout } = useAuth(); // Используем метод logout из контекста

  const handleLogout = async () => {
    try {
      await API.post("/User/logout");
      logout(); // Вызываем метод logout вместо ручного управления
      navigate("/");
    } catch (err) {
      console.error("Ошибка выхода:", err);
    }
  };

  return (
    <div className="min-h-screen flex items-center justify-center bg-white">
      <div className="max-w-md w-full p-6">
        <button
          onClick={handleLogout}
          className="w-full bg-red-500 text-white py-2 rounded-xl hover:opacity-90 transition-colors"
        >
          Выйти
        </button>
      </div>
    </div>
  );
};

export default Profile;
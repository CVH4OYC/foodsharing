// src/pages/Profile.tsx
import { useNavigate } from "react-router-dom";
import { API } from "../services/api";
import { useAuth } from "../context/AuthContext"; // <== добавить

const Profile = () => {
  const navigate = useNavigate();
  const { updateAuth } = useAuth(); // <== добавить

  const handleLogout = async () => {
    try {
      await API.post("/User/logout");
      localStorage.removeItem("token");
      updateAuth(); // <== добавить
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
          className="w-full bg-red-500 text-white py-2 rounded-xl hover:opacity-90"
        >
          Выйти
        </button>
      </div>
    </div>
  );
};

export default Profile;

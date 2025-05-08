// src/pages/Profile.tsx
import { Outlet, NavLink, useNavigate } from "react-router-dom";
import { API } from "../services/api";
import { useAuth } from "../context/AuthContext";

const Profile = () => {
  const { logout } = useAuth();
  const navigate = useNavigate();

  const handleLogout = async () => {
    try {
      await API.post("/User/logout");
      logout();
      navigate("/");
    } catch (err) {
      console.error("Ошибка выхода:", err);
    }
  };

  return (
<div className="flex min-h-screen bg-white py-6 px-4 md:px-0 gap-6">
{/* Боковое меню */}
<aside className="w-64 bg-white shadow-md p-6 hidden md:block rounded-xl">
        <h2 className="text-xl font-bold mb-6">Профиль</h2>
        <ul className="space-y-3">
          <li>
            <NavLink
              to="ads"
              className={({ isActive }) =>
                `block text-left w-full ${isActive ? "text-primary font-semibold" : "text-gray-700"}`
              }
            >
              Мои объявления
            </NavLink>
          </li>
          <li>
            <NavLink
              to="exchanges"
              className={({ isActive }) =>
                `block text-left w-full ${isActive ? "text-primary font-semibold" : "text-gray-700"}`
              }
            >
              Мои обмены
            </NavLink>
          </li>
          <li className="border-t pt-4">
            <NavLink
              to="settings"
              className={({ isActive }) =>
                `block text-left w-full ${isActive ? "text-primary font-semibold" : "text-gray-700"}`
              }
            >
              Настройки
            </NavLink>
          </li>
          <li>
            <button
              onClick={handleLogout}
              className="text-red-500 hover:underline mt-2 text-left w-full"
            >
              Выйти
            </button>
          </li>
        </ul>
      </aside>
  
      {/* Контент секции профиля */}
      <main className="flex-1">
        <Outlet />
      </main>
    </div>
  );
};

export default Profile;

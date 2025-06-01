import { useState } from "react";
import { useNavigate } from "react-router-dom";
import { API } from "../services/api";
import { useAuth } from "../context/AuthContext";

const Login = () => {
  const [userName, setUserName] = useState("");
  const [password, setPassword] = useState("");
  const [error, setError] = useState("");
  const [isLoading, setIsLoading] = useState(false);
  const navigate = useNavigate();
  const { checkAuth } = useAuth();

  const handleLogin = async (e: React.FormEvent) => {
    e.preventDefault();
    setError("");
    setIsLoading(true);

    try {
      await API.post("/user/login", { userName, password }, { withCredentials: true });

      // Попробуем определить местоположение пользователя
      if (navigator.geolocation) {
        navigator.geolocation.getCurrentPosition(
          async (position) => {
            const { latitude, longitude } = position.coords;

            try {
              await API.patch(
                "/user/location",
                { latitude, longitude },
                { withCredentials: true }
              );
            } catch (geoErr) {
              console.warn("Не удалось сохранить координаты:", geoErr);
            }

            await checkAuth();
            navigate("/");
          },
          async (geoError) => {
            console.warn("Геолокация отклонена или недоступна:", geoError);
            await checkAuth();
            navigate("/");
          },
          {
            enableHighAccuracy: true,
            timeout: 5000,
            maximumAge: 0,
          }
        );
      } else {
        console.warn("Геолокация не поддерживается браузером");
        await checkAuth();
        navigate("/");
      }
    } catch (err) {
      setError("Неверное имя пользователя или пароль");
    } finally {
      setIsLoading(false);
    }
  };

  return (
    <div className="bg-white flex items-center justify-center pt-16 pb-12">
      <div className="w-full max-w-md px-4 sm:px-0">
        <div className="bg-white rounded-2xl shadow-xl p-6 sm:p-8">
          <h2 className="text-2xl font-bold text-primary mb-6 text-center">Вход</h2>

          {error && (
            <div className="mb-4 p-3 bg-red-100 text-red-700 rounded-lg">
              {error}
            </div>
          )}

          <form onSubmit={handleLogin} className="space-y-6">
            <div>
              <label className="block text-sm font-medium mb-2">Имя пользователя</label>
              <input
                type="text"
                className="w-full px-4 py-3 border rounded-2xl focus:ring-2 focus:ring-primary"
                value={userName}
                onChange={(e) => setUserName(e.target.value)}
                disabled={isLoading}
              />
            </div>

            <div>
              <label className="block text-sm font-medium mb-2">Пароль</label>
              <input
                type="password"
                className="w-full px-4 py-3 border rounded-2xl focus:ring-2 focus:ring-primary"
                value={password}
                onChange={(e) => setPassword(e.target.value)}
                disabled={isLoading}
              />
            </div>

            <button
              type="submit"
              className="w-full bg-primary text-white py-3 rounded-2xl hover:opacity-90 transition-all disabled:opacity-50"
              disabled={isLoading}
            >
              {isLoading ? "Вход..." : "Войти"}
            </button>
          </form>

          <div className="mt-6 text-center text-sm">
            <span className="text-gray-600">Нет аккаунта? </span>
            <a href="/register" className="text-primary hover:underline font-medium">
              Зарегистрироваться
            </a>
          </div>
        </div>
      </div>
    </div>
  );
};

export default Login;

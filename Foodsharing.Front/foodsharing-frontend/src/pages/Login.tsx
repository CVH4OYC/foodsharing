// src/pages/Login.tsx
import { useState } from "react";
import { useNavigate } from "react-router-dom";
import { API } from "../services/api";
import { useAuth,  } from "../context/AuthContext";

const Login = () => {
  const [userName, setUserName] = useState("");
  const [password, setPassword] = useState("");
  const [error, setError] = useState("");
  const navigate = useNavigate();
  const { login } = useAuth(); // Получаем только метод login

  const handleLogin = async (e: React.FormEvent) => {
    e.preventDefault();
    try {
      const res = await API.post("/User/login", { userName, password });
      
      // Используем метод login из контекста вместо setIsAuth
      login(res.data.token);
      navigate("/");
    } catch (err) {
      setError("Неверное имя пользователя или пароль");
    }
  };

  return (
    <div className="bg-white flex items-center justify-center pt-16 pb-12">
      <div className="w-full max-w-md px-4 sm:px-0">
        <div className="bg-white rounded-2xl shadow-xl p-6 sm:p-8">
          <h2 className="text-2xl font-bold text-primary mb-6 text-center">
            Вход
          </h2>

          {error && (
            <div className="mb-4 p-3 bg-red-100 text-red-700 rounded-lg">
              {error}
            </div>
          )}

          <form onSubmit={handleLogin} className="space-y-6">
            <div>
              <label className="block text-sm font-medium mb-2">
                Имя пользователя
              </label>
              <input
                type="text"
                className="w-full px-4 py-3 border rounded-2xl focus:ring-2 focus:ring-primary"
                value={userName}
                onChange={(e) => setUserName(e.target.value)}
              />
            </div>

            <div>
              <label className="block text-sm font-medium mb-2">
                Пароль
              </label>
              <input
                type="password"
                className="w-full px-4 py-3 border rounded-2xl focus:ring-2 focus:ring-primary"
                value={password}
                onChange={(e) => setPassword(e.target.value)}
              />
            </div>

            <button
              type="submit"
              className="w-full bg-primary text-white py-3 rounded-2xl hover:opacity-90 transition-all"
            >
              Войти
            </button>
          </form>

          <div className="mt-6 text-center text-sm">
            <span className="text-gray-600">Нет аккаунта? </span>
            <a
              href="/register"
              className="text-primary hover:underline font-medium"
            >
              Зарегистрироваться
            </a>
          </div>
        </div>
      </div>
    </div>
  );
};

export default Login;

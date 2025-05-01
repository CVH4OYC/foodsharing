// src/pages/Login.tsx
import { useState } from "react";
import { useNavigate } from "react-router-dom";
import { API } from "../services/api";

const Login = () => {
  const [userName, setUserName] = useState("");
  const [password, setPassword] = useState("");
  const navigate = useNavigate();
  const [error, setError] = useState("");

  const handleLogin = async (e: React.FormEvent) => {
    e.preventDefault();
    try {
      const res = await API.post("/User/login", { userName, password });
      localStorage.setItem("token", res.data.token);
      navigate("/secure");
    } catch (err) {
      setError("Неверное имя пользователя или пароль");
    }
  };

  return (
    <div className="bg-white min-h-[calc(100vh-100px)] mt-[100px] flex items-center">
      <div className="w-full max-w-[1440px] mx-auto px-[240px]">
        <div className="max-w-md mx-auto bg-white rounded-2xl shadow-lg p-8">
          <h2 className="text-2xl font-bold text-[#4CAF50] mb-6 text-center">
            Вход
          </h2>

          {error && (
            <div className="mb-4 p-3 bg-red-100 text-red-700 rounded-xl">
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
                className="w-full px-4 py-3 border rounded-2xl focus:ring-2 focus:ring-[#4CAF50]"
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
                className="w-full px-4 py-3 border rounded-2xl focus:ring-2 focus:ring-[#4CAF50]"
                value={password}
                onChange={(e) => setPassword(e.target.value)}
              />
            </div>

            <button
              type="submit"
              className="w-full bg-[#4CAF50] text-white py-3 rounded-2xl hover:opacity-90 transition-all"
            >
              Войти
            </button>
          </form>
        </div>
      </div>
    </div>
  );
};

export default Login;
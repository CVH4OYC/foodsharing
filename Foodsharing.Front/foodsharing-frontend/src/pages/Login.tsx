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
    <div className="py-12 bg-gray-50">
      <div className="max-w-md mx-auto bg-white rounded-lg shadow-md p-8">
        <h2 className="text-2xl font-bold text-center mb-6">Вход</h2>
        
        {error && (
          <div className="mb-4 p-3 bg-red-100 text-red-700 rounded">{error}</div>
        )}

        <form onSubmit={handleLogin} className="space-y-4">
          <div>
            <label className="block text-sm font-medium mb-1">Имя пользователя</label>
            <input
              type="text"
              value={userName}
              onChange={(e) => setUserName(e.target.value)}
              className="w-full px-3 py-2 border rounded-md"
              required
            />
          </div>

          <div>
            <label className="block text-sm font-medium mb-1">Пароль</label>
            <input
              type="password"
              value={password}
              onChange={(e) => setPassword(e.target.value)}
              className="w-full px-3 py-2 border rounded-md"
              required
            />
          </div>

          <button
            type="submit"
            className="w-full bg-blue-500 text-white py-2 px-4 rounded-md hover:bg-blue-600"
          >
            Войти
          </button>
        </form>

        <div className="mt-6 text-center">
          <span className="text-gray-600">Нет аккаунта? </span>
          <a href="/register" className="text-blue-500 hover:underline">
            Зарегистрироваться
          </a>
        </div>
      </div>
    </div>
  );
};

export default Login;
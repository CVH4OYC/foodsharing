import { useState } from "react";
import { login } from "../services/authService";

export const Login = () => {
  const [userName, setUserName] = useState("");
  const [password, setPassword] = useState("");

  const handleLogin = async () => {
    try {
      const message = await login(userName, password);
      alert("Успешный вход: " + message);
    } catch (err: any) {
      alert("Ошибка входа: " + err.response?.data || err.message);
    }
  };

  return (
    <div>
      <h2>Вход</h2>
      <input value={userName} onChange={e => setUserName(e.target.value)} placeholder="Имя пользователя" />
      <input type="password" value={password} onChange={e => setPassword(e.target.value)} placeholder="Пароль" />
      <button onClick={handleLogin}>Войти</button>
    </div>
  );
};
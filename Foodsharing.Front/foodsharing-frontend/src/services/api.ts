import axios from "axios";

const API = axios.create({
  baseURL: "https://localhost:7044/api", // адрес Web API
  withCredentials: true, 
});

// Добавляем токен в заголовки
API.interceptors.request.use(config => {
  const token = localStorage.getItem("token");
  if (token) {
    config.headers.Authorization = `Bearer ${token}`;
  }
  return config;
});

export { API };

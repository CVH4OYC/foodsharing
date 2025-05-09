// src/services/api.ts
import axios from "axios";

// Для API-запросов
const API = axios.create({
  baseURL: "https://localhost:7044/api",
  withCredentials: true, // включаем отправку cookies
});

// Для статических файлов
const StaticAPI = axios.create({
  baseURL: "https://localhost:7044",
  withCredentials: true,
});

// Глобальный перехват ошибок, например, 401
const handleUnauthorized = (error: any) => {
  if (error.response?.status === 401) {
    console.warn("Unauthorized — handled elsewhere in context or routes.");
    // Не редиректим, просто пробрасываем ошибку
  }
  return Promise.reject(error);
};

API.interceptors.response.use(undefined, handleUnauthorized);
StaticAPI.interceptors.response.use(undefined, handleUnauthorized);

export { API, StaticAPI };

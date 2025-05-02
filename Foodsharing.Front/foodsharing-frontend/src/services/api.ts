// src/services/api.ts
import axios from "axios";

// Для API-запросов
const API = axios.create({
  baseURL: "https://localhost:7044/api",
  withCredentials: true,
});

// Для статических файлов
const StaticAPI = axios.create({
  baseURL: "https://localhost:7044", // Без /api в конце
  withCredentials: true,
});

// Общий интерцептор для авторизации
const setAuthHeader = (config: any) => {
  const token = localStorage.getItem("token");
  if (token) {
    config.headers.Authorization = `Bearer ${token}`;
  }
  return config;
};

API.interceptors.request.use(setAuthHeader);
StaticAPI.interceptors.request.use(setAuthHeader);

export { API, StaticAPI };
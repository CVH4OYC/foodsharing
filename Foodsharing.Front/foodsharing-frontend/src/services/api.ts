import axios from "axios";

export const API = axios.create({
  baseURL: "https://localhost:7044/api", // адрес  ASP.NET API
  withCredentials: true, 
});
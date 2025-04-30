import { API } from "./api";

export const login = async (userName: string, password: string) => {
  const res = await API.post("/user/login", { userName, password });
  return res.data;
};
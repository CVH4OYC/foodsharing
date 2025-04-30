import { useEffect, useState } from "react";
import { API } from "../services/api";

export const SecurePage = () => {
  const [data, setData] = useState("");

  useEffect(() => {
    API.get("/Test/test") // защищённый эндпоинт
      .then(res => setData(res.data))
      .catch(() => setData("Нет доступа / ошибка авторизации"));
  }, []);

  return (
    <div>
      <h1>Защищённая страница</h1>
      <p>{data}</p>
    </div>
  );
};

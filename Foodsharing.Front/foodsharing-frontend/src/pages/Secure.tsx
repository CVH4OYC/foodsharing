import { useEffect, useState } from "react";
import { API } from "../services/api";

const SecurePage = () => {
  const [data, setData] = useState("");

  useEffect(() => {
    API.get("/Test/test")
      .then(res => setData(res.data))
      .catch(() => setData("Нет доступа / ошибка авторизации"));
  }, []);

  return (
    <div className="p-6">
      <h1 className="text-2xl font-bold">Защищённая страница</h1>
      <p className="mt-4">{data}</p>
    </div>
  );
};

export default SecurePage;

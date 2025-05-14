import { useParams, Link } from "react-router-dom";
import { useEffect, useState } from "react";
import { API } from "../services/api";

const OrgRepresentativesPage = () => {
  const { orgId } = useParams();
  const [representatives, setRepresentatives] = useState([]);
  const [credentials, setCredentials] = useState<{ userName: string; password: string } | null>(null);

  const fetchRepresentatives = async () => {
    try {
      const res = await API.get(`/organization/representatives/${orgId}`);
      setRepresentatives(res.data);
    } catch (err) {
      console.error("Ошибка загрузки представителей", err);
    }
  };

  const handleCreateRepresentative = async () => {
    try {
      const res = await API.post("/organization/createRepresentative", null, {
        params: { orgId }
      });
      setCredentials(res.data);
      fetchRepresentatives();
    } catch (err) {
      console.error("Ошибка создания представителя", err);
    }
  };

  useEffect(() => {
    fetchRepresentatives();
  }, [orgId]);

  return (
    <div>
      <h1 className="text-2xl font-bold mb-4">Представители</h1>

      <button
        onClick={handleCreateRepresentative}
        className="mb-4 bg-primary hover:bg-green-600 text-white px-4 py-2 rounded-xl transition"
      >
        Назначить представителя
      </button>

      {credentials && (
        <div className="mb-4 p-4 bg-green-50 border border-green-400 rounded">
          <p><strong>Логин:</strong> <code>{credentials.userName}</code></p>
          <p><strong>Пароль:</strong> <code>{credentials.password}</code></p>
        </div>
      )}

      <ul className="space-y-2">
        {representatives.map((rep: any) => (
          <li key={rep.userId} className="border-b pb-2">
            <Link
              to={`/profile/user/${rep.userId}`}
              className="text-primary hover:underline"
            >
              {rep.firstName} {rep.lastName || ""} — @{rep.userName}
            </Link>
          </li>
        ))}
      </ul>
    </div>
  );
};

export default OrgRepresentativesPage;

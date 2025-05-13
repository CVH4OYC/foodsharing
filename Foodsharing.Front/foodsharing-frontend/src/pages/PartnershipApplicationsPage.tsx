import { useEffect, useState } from "react";
import { API } from "../services/api";

const PartnershipApplicationsPage = () => {
  const [applications, setApplications] = useState([]);
  const [search, setSearch] = useState("");
  const [sortBy, setSortBy] = useState("dateDesc");
  const [statusFilter, setStatusFilter] = useState("");
  const [page, setPage] = useState(1);
  const [limit] = useState(10);

  useEffect(() => {
    const fetchApplications = async () => {
      try {
        const res = await API.get("/parthnership/applications", {
          params: { search, sortBy, page, limit, statusFilter },
        });
        setApplications(res.data);
      } catch (err) {
        console.error("Ошибка при загрузке заявок", err);
      }
    };

    fetchApplications();
  }, [search, sortBy, page, statusFilter]);

  return (
    <div className="w-full mt-8 min-h-[80vh] flex flex-col">
      <h1 className="text-2xl font-bold mb-6">Заявки на партнёрство</h1>

      <div className="flex flex-wrap gap-4 mb-6">
        {/* Поиск */}
        <div className="flex-1 min-w-[200px]">
          <div className="flex gap-2 border border-primary rounded-xl px-4 py-2 items-center w-full">
            <input
              type="text"
              placeholder="Поиск по названию организации"
              className="outline-none text-sm w-full bg-transparent"
              value={search}
              onChange={(e) => setSearch(e.target.value)}
            />
            <svg className="w-5 h-5 text-gray-500" fill="none" stroke="currentColor" viewBox="0 0 24 24">
              <path
                strokeLinecap="round"
                strokeLinejoin="round"
                strokeWidth={2}
                d="M21 21l-6-6m2-5a7 7 0 11-14 0 7 7 0 0114 0z"
              />
            </svg>
          </div>
        </div>

        {/* Статус */}
        <select
          value={statusFilter}
          onChange={(e) => setStatusFilter(e.target.value)}
          className="border border-primary rounded-xl px-4 py-2 text-sm bg-white focus:outline-none appearance-none pr-8"
          style={{
            backgroundImage:
              "url(\"data:image/svg+xml;base64,PHN2ZyB3aWR0aD0iMTYiIGhlaWdodD0iMTYiIHZpZXdCb3g9IjAgMCAyNCAyNCIgZmlsbD0ibm9uZSIgeG1sbnM9Imh0dHA6Ly93d3cudzMub3JnLzIwMDAvc3ZnIj48cGF0aCBkPSJNNiA5bDYgNiA2LTYiIHN0cm9rZT0iIzM2MzYzNiIgc3Ryb2tlLXdpZHRoPSIyIiBzdHJva2UtbGluZWNhcD0icm91bmQiIHN0cm9rZS1saW5lam9pbj0icm91bmQiLz48L3N2Zz4=\")",
            backgroundRepeat: "no-repeat",
            backgroundPosition: "calc(100% - 1rem) center",
          }}
        >
          <option value="">Все</option>
          <option value="isPending">На рассмотрении</option>
          <option value="isReviewed">Рассмотренные</option>
        </select>

        {/* Сортировка */}
        <select
          value={sortBy}
          onChange={(e) => setSortBy(e.target.value)}
          className="border border-primary rounded-xl px-4 py-2 text-sm bg-white focus:outline-none appearance-none pr-8"
          style={{
            backgroundImage:
              "url(\"data:image/svg+xml;base64,PHN2ZyB3aWR0aD0iMTYiIGhlaWdodD0iMTYiIHZpZXdCb3g9IjAgMCAyNCAyNCIgZmlsbD0ibm9uZSIgeG1sbnM9Imh0dHA6Ly93d3cudzMub3JnLzIwMDAvc3ZnIj48cGF0aCBkPSJNNiA5bDYgNiA2LTYiIHN0cm9rZT0iIzM2MzYzNiIgc3Ryb2tlLXdpZHRoPSIyIiBzdHJva2UtbGluZWNhcD0icm91bmQiIHN0cm9rZS1saW5lam9pbj0icm91bmQiLz48L3N2Zz4=\")",
            backgroundRepeat: "no-repeat",
            backgroundPosition: "calc(100% - 1rem) center",
          }}
        >
          <option value="dateDesc">Сначала новые</option>
          <option value="dateAsc">Сначала старые</option>
          <option value="name">По названию</option>
        </select>
      </div>

      {/* Заголовки колонок */}
      <div className="hidden md:grid grid-cols-4 gap-4 bg-gray-100 px-4 py-3 font-medium text-gray-600 rounded-t-xl text-sm">
        <div>№</div>
        <div>Организация</div>
        <div>Дата</div>
        <div>Статус</div>
      </div>

      {/* Строки заявок */}
      {applications.map((app: any, index) => (
        <div
          key={app.organizationId}
          className="grid grid-cols-1 md:grid-cols-4 gap-4 items-center border-b px-4 py-3 text-sm hover:bg-gray-50 transition"
        >
          <div className="font-medium text-gray-700">
            {(page - 1) * limit + index + 1}
          </div>
          <div>{app.organization?.name || "—"}</div>
          <div>{app.submittedAt ? new Date(app.submittedAt).toLocaleDateString("ru-RU") : "—"}</div>
          <div className="text-sm text-gray-600">{app.status || "Неизвестно"}</div>
        </div>
      ))}

      {/* Навигация */}
      <div className="mt-auto pt-10 pb-12 flex justify-between items-center">
        <button
          onClick={() => setPage((p) => Math.max(1, p - 1))}
          disabled={page === 1}
          className="px-4 py-2 bg-gray-200 rounded-xl hover:bg-gray-300 disabled:opacity-50"
        >
          Назад
        </button>
        <span>Страница {page}</span>
        <button
          onClick={() => setPage((p) => p + 1)}
          className="px-4 py-2 bg-gray-200 rounded-xl hover:bg-gray-300"
        >
          Вперёд
        </button>
      </div>
    </div>
  );
};

export default PartnershipApplicationsPage;
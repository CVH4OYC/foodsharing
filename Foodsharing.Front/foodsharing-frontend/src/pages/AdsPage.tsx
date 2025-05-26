import { useEffect, useState, useRef, useCallback, useMemo } from "react";
import { API, StaticAPI } from "../services/api";
import { Category, Announcement } from "../types/ads";
import { useNavigate, useSearchParams } from "react-router-dom";
import AdCard from "../components/AdCard";

const AdsPage = () => {
  const [categories, setCategories] = useState<Category[]>([]);
  const [announcements, setAnnouncements] = useState<Announcement[]>([]);
  const [page, setPage] = useState(1);
  const [loading, setLoading] = useState(false);
  const [selectedCategory, setSelectedCategory] = useState<string | null>(null);
  const [sortBy, setSortBy] = useState("dateCreation");
  const [search, setSearch] = useState("");
  const [viewMode, setViewMode] = useState<"list" | "map">("list");
  const [lastUpdate, setLastUpdate] = useState(Date.now());
  const [mobileFiltersOpen, setMobileFiltersOpen] = useState(false);
  const [mobileSortOpen, setMobileSortOpen] = useState(false);
  const [statusFilter, setStatusFilter] = useState<string | null>("all");
  const [isInitialized, setIsInitialized] = useState(false);

  const loaderRef = useRef<HTMLDivElement | null>(null);
  const navigate = useNavigate();
  const [searchParams] = useSearchParams();

  const urlCategory = useMemo(() => searchParams.get("category"), [searchParams]);

  const fetchCategories = async () => {
    try {
      const res = await API.get("/category");
      const flat = res.data;
      const tree = flat
        .filter((c: Category) => !c.parentId)
        .map((parent: Category) => ({
          ...parent,
          children: flat.filter((child: Category) => child.parentId === parent.id),
        }));
      setCategories(tree);
    } catch (err) {
      console.error("Ошибка загрузки категорий", err);
    }
  };

  const fetchAnnouncements = useCallback(
    async (reset = false) => {
      try {
        setLoading(true);
        const res = await API.get("/announcement", {
          params: {
            page: reset ? 1 : page,
            limit: 10,
            categoryId: selectedCategory,
            sortBy,
            search,
            statusFilter: statusFilter ?? undefined,
          },
        });
        setAnnouncements((prev) => (reset ? res.data : [...prev, ...res.data]));
        if (reset) setPage(2);
      } catch (err) {
        console.error("Ошибка загрузки объявлений", err);
      } finally {
        setLoading(false);
        setLastUpdate(Date.now());
      }
    },
    [page, selectedCategory, sortBy, search, statusFilter]
  );

  // 🟡 Первый запуск: применяем фильтр из URL и грузим категории
  useEffect(() => {
    if (urlCategory) {
      setSelectedCategory(urlCategory);
    }
    setIsInitialized(true);
    fetchCategories();
  }, []);

  // 🟡 Подгрузка при скролле
  useEffect(() => {
    const observer = new IntersectionObserver(
      ([entry]) => {
        if (entry.isIntersecting && !loading && announcements.length > 0) {
          setPage((prev) => prev + 1);
        }
      },
      { threshold: 0.1 }
    );
    const current = loaderRef.current;
    if (current) observer.observe(current);
    return () => {
      if (current) observer.unobserve(current);
    };
  }, [loading, announcements.length]);

  // 🟢 Подгрузка объявлений при изменении фильтров (после инициализации)
  useEffect(() => {
    if (isInitialized) {
      fetchAnnouncements(true);
    }
  }, [selectedCategory, sortBy, search, statusFilter, isInitialized]);

  // 🔁 Автообновление раз в 60 сек.
  useEffect(() => {
    const interval = setInterval(() => {
      if (Date.now() - lastUpdate >= 60000) {
        fetchAnnouncements(true);
      }
    }, 100000);
    return () => clearInterval(interval);
  }, [lastUpdate, fetchAnnouncements]);

  const resetFilters = () => {
    setSelectedCategory(null);
    setSortBy("dateCreation");
    setSearch("");
    setStatusFilter("all");
    navigate("/ads", { replace: true }); // <-- очищает URL
  };

  return (
    <div className="bg-white min-h-screen py-6 px-4 md:px-0 flex flex-col md:flex-row gap-6">
      {/* Категории - десктоп */}
      {/* Боковая панель с фильтрами */}
      <aside className="w-64 hidden md:block bg-white rounded-xl shadow-lg p-4 shrink-0">
        <div className="flex justify-between items-center mb-4">
          <h3 className="text-lg font-semibold">Категории</h3>
          <button onClick={resetFilters} className="text-sm text-primary hover:underline">
            Сбросить фильтры
          </button>
        </div>
        <ul className="space-y-2 text-sm mb-4">
          {categories.map((cat) => (
            <li key={cat.id}>
              <details className="group">
                <summary className="cursor-pointer flex justify-between items-center hover:bg-gray-50 px-2 py-1 rounded-lg">
                  <div className="flex items-center gap-2">
                    <button
                      onClick={(e) => {
                        e.stopPropagation();
                        setSelectedCategory((prev) => (prev === cat.id ? null : cat.id));
                      }}
                      className={`w-4 h-4 rounded-full border-2 ${
                        selectedCategory === cat.id ? "bg-primary border-primary" : "border-gray-300"
                      }`}
                    />
                    <span className="truncate">{cat.name}</span>
                  </div>
                  <svg className="w-4 h-4 text-gray-500 transition-transform group-open:rotate-180" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                    <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M19 9l-7 7-7-7" />
                  </svg>
                </summary>
                {cat.children && (
                  <ul className="pl-8 mt-1 space-y-1">
                    {cat.children.map((child) => (
                      <li key={child.id} className="flex items-center gap-2">
                        <button
                          onClick={() => setSelectedCategory((prev) => (prev === child.id ? null : child.id))}
                          className={`w-3 h-3 rounded-full border-2 ${
                            selectedCategory === child.id ? "bg-primary border-primary" : "border-gray-300"
                          }`}
                        />
                        <span className="truncate">{child.name}</span>
                      </li>
                    ))}
                  </ul>
                )}
              </details>
            </li>
          ))}
        </ul>

        <div className="mt-6">
          <h3 className="text-lg font-semibold mb-2">Статус</h3>
          <ul className="space-y-2 text-sm">
            {[
              { label: "Все", value: "all" },
              { label: "Забронированные", value: "booked" },
              { label: "Свободные", value: "free" },
            ].map(({ label, value }) => (
              <li key={label}>
                <button
                  onClick={() => setStatusFilter(value)}
                  className="w-full text-left flex items-center gap-2 px-2 py-1 rounded-lg hover:bg-gray-50"
                >
                  <span
                    className={`w-4 h-4 rounded-full border-2 flex-shrink-0 ${
                      statusFilter === value ? "bg-primary border-primary" : "border-gray-300"
                    }`}
                  ></span>
                  {label}
                </button>
              </li>
            ))}
          </ul>
        </div>
      </aside>

      {/* Основной контент */}
      <main className="flex-1">
        {/* Шапка */}
        <div className="flex flex-wrap gap-4 items-center mb-6 justify-between">
          <button
            onClick={() => navigate("/ads/new")}
            className="bg-primary text-white px-4 py-2 rounded-xl flex items-center gap-2 hover:bg-primary-dark transition-colors"
          >
            Создать
            <svg className="w-4 h-4" fill="none" stroke="currentColor" viewBox="0 0 24 24">
              <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M12 6v6m0 0v6m0-6h6m-6 0H6" />
            </svg>
          </button>

          {/* Только в мобильной версии */}
          <div className="flex md:hidden gap-2 w-full justify-between">
            <button
              onClick={() => setMobileFiltersOpen((p) => !p)}
              className="border border-primary text-sm px-4 py-2 rounded-xl"
            >
              Категории
            </button>
            <button
              onClick={() => setMobileSortOpen((p) => !p)}
              className="border border-primary text-sm px-4 py-2 rounded-xl"
            >
              Сортировка
            </button>
          </div>

          {/* Список / Карта */}
          <div className="flex gap-0 border border-primary rounded-xl overflow-hidden hidden md:flex">
            <button
              onClick={() => setViewMode("list")}
              className={`px-4 py-2 transition-colors ${
                viewMode === "list" ? "bg-primary text-white" : "bg-white text-gray-700"
              }`}
            >
              Список
            </button>
            <button
              onClick={() => setViewMode("map")}
              className={`px-4 py-2 border-l border-primary transition-colors ${
                viewMode === "map" ? "bg-primary text-white" : "bg-white text-gray-700"
              }`}
            >
              Карта
            </button>
          </div>

          {/* Поиск */}
          <div className="flex-1 min-w-[200px]">
            <div className="flex gap-2 border border-primary rounded-xl px-4 py-2 items-center w-full">
              <input
                type="text"
                placeholder="Поиск объявлений..."
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

          {/* Сортировка - десктоп */}
          <select
            value={sortBy}
            onChange={(e) => setSortBy(e.target.value)}
            className="hidden md:block border border-primary rounded-xl px-4 py-2 text-sm bg-white focus:outline-none appearance-none pr-8"
            style={{
              backgroundImage:
                "url(\"data:image/svg+xml;base64,PHN2ZyB3aWR0aD0iMTYiIGhlaWdodD0iMTYiIHZpZXdCb3g9IjAgMCAyNCAyNCIgZmlsbD0ibm9uZSIgeG1sbnM9Imh0dHA6Ly93d3cudzMub3JnLzIwMDAvc3ZnIj48cGF0aCBkPSJNNiA5bDYgNiA2LTYiIHN0cm9rZT0iIzM2MzYzNiIgc3Ryb2tlLXdpZHRoPSIyIiBzdHJva2UtbGluZWNhcD0icm91bmQiIHN0cm9rZS1saW5lam9pbj0icm91bmQiLz48L3N2Zz4=\")",
              backgroundRepeat: "no-repeat",
              backgroundPosition: "calc(100% - 1rem) center",
            }}
          >
            <option value="dateCreation">Новинки</option>
            <option value="expirationDate">По сроку годности</option>
            <option value="title">По названию</option>
          </select>
        </div>

        {/* Мобильный блок сортировки */}
        {mobileSortOpen && (
          <div className="md:hidden mb-4 space-y-2">
            <button onClick={() => setSortBy("dateCreation")} className="block w-full text-left px-4 py-2 border rounded-xl">
              Новинки
            </button>
            <button onClick={() => setSortBy("expirationDate")} className="block w-full text-left px-4 py-2 border rounded-xl">
              По сроку годности
            </button>
            <button onClick={() => setSortBy("title")} className="block w-full text-left px-4 py-2 border rounded-xl">
              По названию
            </button>
          </div>
        )}

        {/* Мобильный блок категорий */}
        {mobileFiltersOpen && (
          <div className="md:hidden mb-4">
            <ul className="space-y-2 text-sm">
              {categories.map((cat) => (
                <li key={cat.id}>
                  <details className="group">
                    <summary className="cursor-pointer flex justify-between items-center hover:bg-gray-50 px-2 py-1 rounded-lg">
                      <div className="flex items-center gap-2">
                        <button
                          onClick={(e) => {
                            e.stopPropagation();
                            setSelectedCategory((prev) => (prev === cat.id ? null : cat.id));
                          }}
                          className={`w-4 h-4 rounded-full border-2 ${
                            selectedCategory === cat.id ? "bg-primary border-primary" : "border-gray-300"
                          }`}
                        />
                        <span className="truncate">{cat.name}</span>
                      </div>
                      <svg
                        className="w-4 h-4 text-gray-500 transition-transform group-open:rotate-180"
                        fill="none"
                        stroke="currentColor"
                        viewBox="0 0 24 24"
                      >
                        <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M19 9l-7 7-7-7" />
                      </svg>
                    </summary>
                  </details>
                </li>
              ))}
            </ul>
          </div>
        )}

        {/* Объявления */}
        <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 gap-4">
          {announcements.map((ad) => {
            const owner = ad.user ?? ad.organization;
            const ownerLink = ad.user
              ? `/profile/user/${ad.user.userId}`
              : ad.organization
              ? `/organizations/${ad.organization.id}`
              : null;
            const ownerImage = ad.user?.image
              ? `${StaticAPI.defaults.baseURL}${ad.user.image}`
              : ad.organization?.logoImage
              ? `${StaticAPI.defaults.baseURL}${ad.organization.logoImage}`
              : "/default-avatar.png";
            const ownerName = ad.user
              ? `${ad.user.firstName || ""} ${ad.user.lastName || ""}`.trim()
              : ad.organization?.name ?? "Неизвестно";

            return (
              <AdCard
                key={ad.announcementId}
                {...ad}
                image={`${StaticAPI.defaults.baseURL}${ad.image}`}
                categoryColor={ad.category?.color || "#4CAF50"}
                date={new Date(ad.dateCreation).toLocaleDateString("ru-RU")}
                owner={{
                  name: ownerName,
                  image: ownerImage,
                  link: ownerLink,
                }}
              />
            );
          })}
        </div>

        {/* Лоадер */}
        <div ref={loaderRef} className="h-12 mt-6 flex justify-center items-center text-sm text-gray-500">
          {loading && <span>Загрузка...</span>}
        </div>
      </main>
    </div>
  );
};

export default AdsPage;

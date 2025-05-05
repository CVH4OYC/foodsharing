import { useEffect, useState, useRef, useCallback } from "react";
import { API, StaticAPI } from "../services/api";
import { Category, Announcement } from "../types/ads";
import { useNavigate } from "react-router-dom";
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
  const loaderRef = useRef<HTMLDivElement | null>(null);
  const navigate = useNavigate();

  const fetchCategories = async () => {
    try {
      const res = await API.get("/category");
      const flat = res.data;
      const tree = flat.filter((c: Category) => !c.parentId)
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
          },
        });
        setAnnouncements(prev =>
          reset ? res.data : [...prev, ...res.data]
        );
        if (reset) setPage(2);
      } catch (err) {
        console.error("Ошибка загрузки объявлений", err);
      } finally {
        setLoading(false);
        setLastUpdate(Date.now());
      }
    },
    [page, selectedCategory, sortBy, search]
  );

  useEffect(() => {
    fetchCategories();
    fetchAnnouncements(true);
    const interval = setInterval(() => {
      if (Date.now() - lastUpdate >= 60000) {
        fetchAnnouncements(true);
      }
    }, 10000);
    return () => clearInterval(interval);
  }, []);

  useEffect(() => {
    const observer = new IntersectionObserver(
      ([entry]) => {
        if (entry.isIntersecting && !loading && announcements.length > 0) {
          setPage(prev => prev + 1);
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

  useEffect(() => {
    fetchAnnouncements(true);
  }, [selectedCategory, sortBy, search]);

  const resetFilters = () => {
    setSelectedCategory(null);
    setSortBy("dateCreation");
    setSearch("");
  };

  return (
    <div className="bg-white min-h-screen py-6 flex gap-6">
      <aside className="w-64 hidden md:block bg-white rounded-xl shadow-lg p-4">
        <div className="flex justify-between items-center mb-4">
          <h3 className="text-lg font-semibold">Категории</h3>
          <button
            onClick={resetFilters}
            className="text-sm text-primary hover:underline"
          >
            Сбросить
          </button>
        </div>
        <ul className="space-y-2 text-sm">
          {categories.map(cat => (
            <li key={cat.id}>
              <details className="group">
                <summary className="cursor-pointer flex justify-between items-center hover:bg-gray-50 px-2 py-1 rounded-lg">
                  <div className="flex items-center gap-2">
                    <button
                      onClick={(e) => {
                        e.stopPropagation();
                        setSelectedCategory(prev => prev === cat.id ? null : cat.id);
                      }}
                      className={`w-4 h-4 rounded-full border-2 ${selectedCategory === cat.id ? "bg-primary border-primary" : "border-gray-300"}`}
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
                {cat.children && (
                  <ul className="pl-8 mt-1 space-y-1">
                    {cat.children.map(child => (
                      <li key={child.id} className="flex items-center gap-2">
                        <button
                          onClick={() => setSelectedCategory(prev => prev === child.id ? null : child.id)}
                          className={`w-3 h-3 rounded-full border-2 ${selectedCategory === child.id ? "bg-primary border-primary" : "border-gray-300"}`}
                        />
                        <button className="hover:text-primary text-left truncate">
                          {child.name}
                        </button>
                      </li>
                    ))}
                  </ul>
                )}
              </details>
            </li>
          ))}
        </ul>
      </aside>

      <main className="flex-1">
        <div className="flex flex-wrap gap-4 items-center mb-6">
          <button
            onClick={() => navigate("/ads/new")}
            className="bg-primary text-white px-4 py-2 rounded-xl flex items-center gap-2 hover:bg-primary-dark transition-colors"
          >
            Создать
            <svg className="w-4 h-4" fill="none" stroke="currentColor" viewBox="0 0 24 24">
              <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M12 6v6m0 0v6m0-6h6m-6 0H6" />
            </svg>
          </button>

          <div className="flex gap-0 border border-primary rounded-xl overflow-hidden">
            <button
              onClick={() => setViewMode("list")}
              className={`px-4 py-2 transition-colors ${viewMode === "list" ? "bg-primary text-white" : "bg-white text-gray-700"}`}
            >
              Список
            </button>
            <button
              onClick={() => setViewMode("map")}
              className={`px-4 py-2 border-l border-primary transition-colors ${viewMode === "map" ? "bg-primary text-white" : "bg-white text-gray-700"}`}
            >
              Карта
            </button>
          </div>

          <div className="flex-1 min-w-[200px]">
            <div className="flex gap-2 border border-primary rounded-xl px-4 py-2 items-center w-full">
              <input
                type="text"
                placeholder="Поиск объявлений..."
                className="outline-none text-sm w-full bg-transparent"
                value={search}
                onChange={e => setSearch(e.target.value)}
              />
              <svg className="w-5 h-5 text-gray-500" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M21 21l-6-6m2-5a7 7 0 11-14 0 7 7 0 0114 0z" />
              </svg>
            </div>
          </div>

          <select
            value={sortBy}
            onChange={e => setSortBy(e.target.value)}
            className="border border-primary rounded-xl px-4 py-2 text-sm bg-white focus:outline-none appearance-none pr-8"
            style={{
              backgroundImage: "url(\"data:image/svg+xml;base64,PHN2ZyB3aWR0aD0iMTYiIGhlaWdodD0iMTYiIHZpZXdCb3g9IjAgMCAyNCAyNCIgZmlsbD0ibm9uZSIgeG1sbnM9Imh0dHA6Ly93d3cudzMub3JnLzIwMDAvc3ZnIj48cGF0aCBkPSJNNiA5bDYgNiA2LTYiIHN0cm9rZT0iIzM2MzYzNiIgc3Ryb2tlLXdpZHRoPSIyIiBzdHJva2UtbGluZWNhcD0icm91bmQiIHN0cm9rZS1saW5lam9pbj0icm91bmQiLz48L3N2Zz4=\")",
              backgroundRepeat: "no-repeat",
              backgroundPosition: "calc(100% - 1rem) center",
            }}
          >
            <option value="dateCreation">Новинки</option>
            <option value="expirationDate">По сроку годности</option>
            <option value="title">По названию</option>
          </select>
        </div>

        <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 gap-4">
          {announcements.map(ad => (
            <AdCard
              key={ad.announcementId}
              {...ad}
              image={`${StaticAPI.defaults.baseURL}${ad.image}`}
              user={{
                ...ad.user,
                image: ad.user.image ? `${StaticAPI.defaults.baseURL}${ad.user.image}` : undefined,
              }}
              categoryColor={ad.category?.color || "#4CAF50"}
              date={new Date(ad.dateCreation).toLocaleDateString("ru-RU")}
            />
          ))}
        </div>

        <div
          ref={loaderRef}
          className="h-12 mt-6 flex justify-center items-center text-sm text-gray-500"
        >
          {loading && <span>Загрузка...</span>}
        </div>
      </main>
    </div>
  );
};

export default AdsPage;

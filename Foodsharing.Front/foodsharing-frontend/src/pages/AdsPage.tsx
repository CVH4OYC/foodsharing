import { useEffect, useState, useRef, useCallback } from "react";
import { API, StaticAPI } from "../services/api";
import { Category, Announcement } from '../types/ads';
import { useNavigate } from "react-router-dom"; 
import AdCard from "../components/AdCard";

const AdsPage = () => {
  const [categories, setCategories] = useState<Category[]>([]);
  const [announcements, setAnnouncements] = useState<Announcement[]>([]);
  const [page, setPage] = useState(1);
  const [loading, setLoading] = useState(false);
  const [selectedCategory, setSelectedCategory] = useState<string | null>(null);
  const [viewMode, setViewMode] = useState<"list" | "map">("list");
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
    async (isInitial = false) => {
      try {
        setLoading(true);
        const res = await API.get("/Announcement", {
          params: { page, limit: 10 },
        });

        setAnnouncements((prev) =>
          isInitial ? res.data : [...prev, ...res.data]
        );
      } catch (err) {
        console.error("Ошибка загрузки объявлений", err);
      } finally {
        setLoading(false);
      }
    },
    [page]
  );

  useEffect(() => {
    let intervalId: NodeJS.Timeout;

    const initialFetch = async () => {
      await fetchCategories();
      await fetchAnnouncements(true);
    };

    initialFetch();
    intervalId = setInterval(() => {
      fetchCategories();
      fetchAnnouncements(true);
    }, 60000);

    return () => {
      clearInterval(intervalId);
    };
  }, []);

  useEffect(() => {
    const observer = new IntersectionObserver(
      ([entry]) => {
        if (entry.isIntersecting && !loading && announcements.length > 0) {
          setPage((prev) => prev + 1);
        }
      },
      { threshold: 0.1 }
    );

    const currentLoader = loaderRef.current;
    if (currentLoader) observer.observe(currentLoader);

    return () => {
      if (currentLoader) observer.unobserve(currentLoader);
    };
  }, [loading, announcements.length]);

  return (
    <div className="bg-white min-h-screen py-6 flex gap-6">
        {/* Категории */}
        <aside className="w-64 hidden md:block bg-white rounded-xl shadow-lg p-4">
          <h3 className="text-lg font-semibold mb-4">Категории</h3>
          <ul className="space-y-2 text-sm">
            {categories.map((cat) => (
              <li key={cat.id}>
                <details className="group">
                  <summary className="cursor-pointer flex justify-between items-center hover:bg-gray-50 px-2 py-1 rounded-lg">
                    <div className="flex items-center gap-2">
                      <button
                        onClick={(e) => {
                          e.stopPropagation();
                          setSelectedCategory(cat.id);
                        }}
                        className={`w-4 h-4 rounded-full border-2 ${
                          selectedCategory === cat.id
                            ? "bg-primary border-primary"
                            : "border-gray-300"
                        }`}
                      />
                      <span className="truncate">{cat.name}</span>
                    </div>
                    <svg
                      className="w-4 h-4 transform group-open:rotate-180 transition-transform text-gray-500"
                      fill="none"
                      stroke="currentColor"
                      viewBox="0 0 24 24"
                    >
                      <path
                        strokeLinecap="round"
                        strokeLinejoin="round"
                        strokeWidth={2}
                        d="M19 9l-7 7-7-7"
                      />
                    </svg>
                  </summary>
                  {cat.children && (
                    <ul className="pl-8 mt-1 space-y-1">
                      {cat.children.map((child) => (
                        <li key={child.id} className="flex items-center gap-2">
                          <button
                            onClick={() => setSelectedCategory(child.id)}
                            className={`w-3 h-3 rounded-full border-2 ${
                              selectedCategory === child.id
                                ? "bg-primary border-primary"
                                : "border-gray-300"
                            }`}
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

        {/* Основной контент */}
        <main className="flex-1">
          <div className="flex flex-wrap gap-4 items-center mb-6">
          <button
            onClick={() => navigate("/ads/new")}
            className="bg-primary text-white px-4 py-2 rounded-xl flex items-center gap-2 hover:bg-primary-dark transition-colors"
          >
            Создать
            <svg
              className="w-4 h-4 text-white"
              fill="none"
              stroke="currentColor"
              viewBox="0 0 24 24"
            >
              <path
                strokeLinecap="round"
                strokeLinejoin="round"
                strokeWidth={2}
                d="M12 6v6m0 0v6m0-6h6m-6 0H6"
              />
            </svg>
          </button>

            <div className="flex gap-0 border border-primary rounded-xl overflow-hidden">
              <button
                onClick={() => setViewMode("list")}
                className={`px-4 py-2 transition-colors ${
                  viewMode === "list"
                    ? "bg-primary text-white"
                    : "bg-white text-gray-700"
                }`}
              >
                Список
              </button>
              <button
                onClick={() => setViewMode("map")}
                className={`px-4 py-2 border-l border-primary transition-colors ${
                  viewMode === "map"
                    ? "bg-primary text-white"
                    : "bg-white text-gray-700"
                }`}
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
                />
                <svg
                  className="w-5 h-5 text-gray-500"
                  fill="none"
                  stroke="currentColor"
                  viewBox="0 0 24 24"
                >
                  <path
                    strokeLinecap="round"
                    strokeLinejoin="round"
                    strokeWidth={2}
                    d="M21 21l-6-6m2-5a7 7 0 11-14 0 7 7 0 0114 0z"
                  />
                </svg>
              </div>
            </div>

            <select className="border border-primary rounded-xl px-4 py-2 text-sm bg-white 
              focus:outline-none appearance-none pr-8 bg-[url('data:image/svg+xml;base64,PHN2ZyB3aWR0aD0iMTYiIGhlaWdodD0iMTYiIHZpZXdCb3g9IjAgMCAyNCAyNCIgZmlsbD0ibm9uZSIgeG1sbnM9Imh0dHA6Ly93d3cudzMub3JnLzIwMDAvc3ZnIj48cGF0aCBkPSJNNiA5bDYgNiA2LTYiIHN0cm9rZT0iIzM2MzYzNiIgc3Ryb2tlLXdpZHRoPSIyIiBzdHJva2UtbGluZWNhcD0icm91bmQiIHN0cm9rZS1saW5lam9pbj0icm91bmQiLz48L3N2Zz4=')] 
              bg-no-repeat bg-[center_right_1rem]">
              <option>Новинки</option>
              <option>По дате</option>
              <option>По названию</option>
            </select>
          </div>

          <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 gap-4">
            {announcements.map((ad) => (
              <AdCard
                key={ad.announcementId}
                {...ad}
                image={`${StaticAPI.defaults.baseURL}${ad.image}`}
                user={{
                  ...ad.user,
                  image: ad.user.image 
                    ? `${StaticAPI.defaults.baseURL}${ad.user.image}`
                    : undefined,
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
            {loading && "Загрузка..."}
          </div>
        </main>
    </div>
  );
};

export default AdsPage;
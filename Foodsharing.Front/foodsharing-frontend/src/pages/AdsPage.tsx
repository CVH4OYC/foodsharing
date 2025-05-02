import { useEffect, useState, useRef, useCallback } from "react";
import { API, StaticAPI } from "../services/api";
import AdCard from "../components/AdCard";

interface Category {
  id: string;
  name: string;
  parentId: string | null;
  color: string | null;
  children?: Category[];
}

interface Announcement {
  announcementId: string;
  title: string;
  description: string;
  image: string;
  category: {
    name: string;
    color: string;
  };
  user: {
    userName: string;
    image: string;
  };
  address: {
    city?: string;
    street?: string;
    house?: string;
  };
  dateCreation: string;
}

const AdsPage = () => {
  const [categories, setCategories] = useState<Category[]>([]);
  const [announcements, setAnnouncements] = useState<Announcement[]>([]);
  const [page, setPage] = useState(1);
  const [loading, setLoading] = useState(false);
  const loaderRef = useRef<HTMLDivElement | null>(null);

  const fetchCategories = async () => {
    try {
      const res = await API.get("/Gategory");
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

  const fetchAnnouncements = useCallback(async (isInitial = false) => {
    try {
      setLoading(true);
      const res = await API.get("/Announcement", {
        params: { page, limit: 10 } // Пример параметров пагинации
      });
      
      setAnnouncements(prev => 
        isInitial ? res.data : [...prev, ...res.data]
      );
    } catch (err) {
      console.error("Ошибка загрузки объявлений", err);
    } finally {
      setLoading(false);
    }
  }, [page]);

  // Загрузка данных с интервалом
  useEffect(() => {
    let intervalId: NodeJS.Timeout;
    
    const initialFetch = async () => {
      await fetchCategories();
      await fetchAnnouncements(true);
    };

    // Первый запрос сразу
    initialFetch();

    // Интервал для периодического обновления
    intervalId = setInterval(() => {
      fetchCategories();
      fetchAnnouncements(true);
    }, 60000);

    // Очистка при размонтировании
    return () => {
      clearInterval(intervalId);
    };
  }, []);

  // Бесконечная прокрутка
  useEffect(() => {
    const observer = new IntersectionObserver(
      ([entry]) => {
        if (entry.isIntersecting && !loading && announcements.length > 0) {
          setPage(prev => prev + 1);
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
    <div className="bg-white min-h-screen px-4 py-6 flex gap-6 max-w-7xl mx-auto">
      {/* Левый блок: категории */}
      <aside className="w-64 hidden md:block">
        <h3 className="text-lg font-semibold mb-4">Категории</h3>
        <ul className="space-y-2 text-sm">
          {categories.map((cat) => (
            <li key={cat.id}>
              <details className="group">
                <summary className="cursor-pointer flex justify-between items-center">
                  <span>{cat.name}</span>
                </summary>
                {cat.children && (
                  <ul className="pl-4 mt-2 space-y-1">
                    {cat.children.map((child) => (
                      <li key={child.id}>
                        <button className="hover:text-primary">{child.name}</button>
                      </li>
                    ))}
                  </ul>
                )}
              </details>
            </li>
          ))}
        </ul>
      </aside>

      {/* Правый блок: фильтры + объявления */}
      <main className="flex-1">
        <div className="flex flex-wrap gap-4 items-center mb-6">
          <button className="bg-primary text-white px-4 py-2 rounded-xl">
            Создать
          </button>
          <div className="flex gap-2 border rounded-xl px-3 py-2 items-center">
            <input
              type="text"
              placeholder="Поиск"
              className="outline-none text-sm"
            />
          </div>
          <select className="border rounded-xl px-3 py-2 text-sm">
            <option>Новинки</option>
            <option>По дате</option>
            <option>По названию</option>
          </select>
          <div className="flex gap-2 ml-auto">
            <button className="px-3 py-2 border rounded-xl">Список</button>
            <button className="px-3 py-2 border rounded-xl">Карта</button>
          </div>
        </div>

        <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 gap-4">
          {announcements.map((ad) => (
            <AdCard
              key={ad.announcementId}
              title={ad.title}
              description={ad.description}
              image={`${StaticAPI.defaults.baseURL}${ad.image}`}
              categoryColor={ad.category?.color || "#ccc"}
              categoryName={ad.category?.name}
              author={ad.user?.userName}
              authorImage={`${StaticAPI.defaults.baseURL}${ad.user?.image}`}
              location={[
                ad.address?.city,
                ad.address?.street,
                ad.address?.house,
              ]
                .filter(Boolean)
                .join(", ")}
              date={new Date(ad.dateCreation).toLocaleDateString("ru-RU")}
            />
          ))}
        </div>

        <div ref={loaderRef} className="h-12 mt-6 flex justify-center items-center text-sm text-gray-500">
          {loading && "Загрузка..."}
        </div>
      </main>
    </div>
  );
};

export default AdsPage;
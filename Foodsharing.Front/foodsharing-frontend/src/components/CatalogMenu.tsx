import { useEffect, useState } from "react";
import { API } from "../services/api";
import { HiOutlineHeart, HiHeart } from "react-icons/hi";
import { Category } from "../types/ads";

const CatalogMenu = () => {
  const [categories, setCategories] = useState<(Category & { children: Category[] })[]>([]);
  const [expandedIds, setExpandedIds] = useState<string[]>([]);

  useEffect(() => {
    const fetchData = async () => {
      const response = await API.get("/Category");
      const flat: Category[] = response.data;

      // строим дерево с сохранением isFavorite
      const map = new Map<string, Category & { children: Category[] }>();
      flat.forEach((cat) => map.set(cat.id, { ...cat, children: [] }));

      flat.forEach((cat) => {
        if (cat.parentId && map.has(cat.parentId)) {
          map.get(cat.parentId)!.children.push(map.get(cat.id)!);
        }
      });

      const tree = Array.from(map.values()).filter((c) => !c.parentId);
      setCategories(tree);
    };

    fetchData();
  }, []);

  const handleFavorite = async (categoryId: string, isFavorite: boolean = false) => {
    try {
      if (isFavorite) {
        await API.delete("/Category/favorite", {
          params: { categoryId },
          withCredentials: true,
        });
      } else {
        await API.post("/Category/favorite", null, {
          params: { categoryId },
          withCredentials: true,
        });
      }

      setCategories((prev) =>
        prev.map((cat) => {
          if (cat.id === categoryId) {
            return { ...cat, isFavorite: !isFavorite };
          }
          return {
            ...cat,
            children: cat.children.map((child) =>
              child.id === categoryId ? { ...child, isFavorite: !isFavorite } : child
            ),
          };
        })
      );
    } catch (err) {
      console.error("Ошибка обновления избранного", err);
    }
  };

  const toggleExpand = (id: string) => {
    setExpandedIds((prev) =>
      prev.includes(id) ? prev.filter((v) => v !== id) : [...prev, id]
    );
  };

  return (
    <div className="fixed top-16 left-0 right-0 bottom-0 z-40 bg-white shadow-md border-t border-gray-200">
      <div className="mx-auto px-4 sm:px-6 lg:px-8 max-w-7xl flex h-full">
        {/* Левая колонка */}
        <div className="w-64 border-r border-gray-200 p-4 overflow-y-auto">
          <h2 className="text-lg font-semibold mb-4">Категории</h2>
          <ul className="space-y-2">
            {categories.map((cat) => (
              <li key={cat.id}>
                <details className="group" open={expandedIds.includes(cat.id)}>
                  <summary
                    onClick={(e) => {
                      e.preventDefault();
                      toggleExpand(cat.id);
                    }}
                    className="cursor-pointer flex justify-between items-center hover:bg-gray-50 px-2 py-1 rounded-lg"
                  >
                    <span className="truncate">{cat.name}</span>
                    <svg
                      className="w-4 h-4 text-gray-500 transition-transform group-open:rotate-180"
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
                  <ul className="pl-4 mt-1 space-y-1">
                    {cat.children.map((child) => (
                      <li key={child.id} className="text-sm text-gray-700">
                        {child.name}
                      </li>
                    ))}
                  </ul>
                </details>
              </li>
            ))}
          </ul>
        </div>

        {/* Правая часть с карточками */}
        <div className="flex-1 px-4 sm:px-6 lg:px-8 py-6 overflow-y-auto space-y-6">
          {categories.map((parent) => (
            <div
              key={parent.id}
              className="p-4 rounded-xl relative"
              style={{ backgroundColor: parent.color || "#f0f0f0" }}
            >
              <h3 className="text-xl font-bold mb-4 flex items-center justify-between">
                {parent.name}
                <button
                  onClick={() => handleFavorite(parent.id, parent.isFavorite ?? false)}
                  className="text-right"
                >
                  {parent.isFavorite ? (
                    <HiHeart className="w-5 h-5 text-red-600" />
                  ) : (
                    <HiOutlineHeart className="w-5 h-5 text-black" />
                  )}
                </button>
              </h3>
              <div className="grid grid-cols-2 sm:grid-cols-3 md:grid-cols-4 gap-4">
                {parent.children.map((child) => (
                  <div
                    key={child.id}
                    className="p-4 rounded-xl flex flex-col justify-between relative"
                    style={{ backgroundColor: child.color || "#f9f9f9" }}
                  >
                    <div className="aspect-[4/3] rounded mb-2 overflow-hidden"></div>
                    <span className="font-medium">{child.name}</span>
                    <button
                      onClick={() => handleFavorite(child.id, child.isFavorite ?? false)}
                      className="absolute top-2 right-2"
                    >
                      {child.isFavorite ? (
                        <HiHeart className="w-5 h-5 text-red-600" />
                      ) : (
                        <HiOutlineHeart className="w-5 h-5 text-black" />
                      )}
                    </button>
                  </div>
                ))}
              </div>
            </div>
          ))}
        </div>
      </div>
    </div>
  );
};

export default CatalogMenu;

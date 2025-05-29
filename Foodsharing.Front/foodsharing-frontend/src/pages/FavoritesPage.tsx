import { useEffect, useState } from "react";
import { API, StaticAPI } from "../services/api";
import { Category } from "../types/ads";
import { OrganizationDTO } from "../types/org";
import { HiHeart, HiOutlineHeart } from "react-icons/hi";
import { Link } from "react-router-dom";
import { useNavigate } from "react-router-dom";

const FavoritesPage = () => {
  const [categories, setCategories] = useState<Category[]>([]);
  const [organizations, setOrganizations] = useState<OrganizationDTO[]>([]);
  const [loading, setLoading] = useState(true);
  const navigate = useNavigate();

  useEffect(() => {
    const fetchFavorites = async () => {
      try {
        const [catRes, orgRes] = await Promise.all([
          API.get("/category/favorite"),
          API.get("/organization/favorite"),
        ]);
        setCategories(catRes.data);
        setOrganizations(orgRes.data);
      } catch (err) {
        console.error("Ошибка загрузки избранного:", err);
      } finally {
        setLoading(false);
      }
    };

    fetchFavorites();
  }, []);

  const toggleFavoriteCategory = async (categoryId: string, isFavorite: boolean) => {
    try {
      if (isFavorite) {
        await API.delete("/category/favorite", {
          params: { categoryId },
          withCredentials: true,
        });
      } else {
        await API.post("/category/favorite", null, {
          params: { categoryId },
          withCredentials: true,
        });
      }
      setCategories((prev) =>
        prev.map((cat) =>
          cat.id === categoryId ? { ...cat, isFavorite: !isFavorite } : cat
        )
      );
    } catch (err) {
      console.error("Ошибка изменения избранного (категория)", err);
    }
  };

  const toggleFavoriteOrg = async (orgId: string, isFavorite: boolean) => {
    try {
      if (isFavorite) {
        await API.delete("/organization/favorite", {
          params: { orgId },
          withCredentials: true,
        });
      } else {
        await API.post("/organization/favorite", null, {
          params: { orgId },
          withCredentials: true,
        });
      }
      setOrganizations((prev) =>
        prev.map((org) =>
          org.id === orgId ? { ...org, isFavorite: !isFavorite } : org
        )
      );
    } catch (err) {
      console.error("Ошибка изменения избранного (организация)", err);
    }
  };

  if (loading) {
    return <div className="p-6 text-gray-500">Загрузка...</div>;
  }

  return (
    <div className="max-w-7xl mx-auto px-4 py-8 space-y-10">
      <div>
        <h2 className="text-2xl font-bold mb-4">Избранные категории</h2>
        {categories.length === 0 ? (
          <p className="text-gray-400 italic">Нет избранных категорий</p>
        ) : (
          <div className="grid grid-cols-2 sm:grid-cols-3 md:grid-cols-4 gap-4">
            {categories.map((cat) => (
                <div
                    key={cat.id}
                    onClick={() => navigate(`/ads?category=${cat.id}`)}
                    className="p-4 rounded-xl relative cursor-pointer hover:shadow-md transition"
                    style={{ backgroundColor: cat.color || "#f9f9f9" }}
                >
                <div className="aspect-[4/3] rounded mb-2 overflow-hidden"></div>
                <span className="font-medium">{cat.name}</span>
                <button
                  onClick={(e) => {
                    e.stopPropagation();
                    toggleFavoriteCategory(cat.id, cat.isFavorite ?? false);
                  }}
                  className="absolute top-2 right-2"
                >
                  {cat.isFavorite ? (
                    <HiHeart className="w-5 h-5 text-red-600" />
                  ) : (
                    <HiOutlineHeart className="w-5 h-5 text-black" />
                  )}
                </button>
              </div>
            ))}
          </div>
        )}
      </div>

      <div>
        <h2 className="text-2xl font-bold mb-4">Избранные организации</h2>
        {organizations.length === 0 ? (
          <p className="text-gray-400 italic">Нет избранных организаций</p>
        ) : (
          <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 gap-4">
            {organizations.map((org) => (
            <Link
                key={org.id}
                to={`/organizations/${org.id}`}
                className="p-4 border rounded-xl flex items-center gap-4 relative hover:bg-gray-50 transition group"
            >
                <img
                src={
                    org.logoImage
                    ? `${StaticAPI.defaults.baseURL}${org.logoImage}`
                    : "/default-logo.png"
                }
                alt={org.name}
                className="w-14 h-14 rounded-full object-cover"
                />
                <div className="flex-1">
                <p className="font-semibold group-hover:underline">{org.name}</p>
                <p className="text-sm text-gray-500">{org.organizationForm}</p>
                </div>
                <button
                onClick={(e) => {
                    e.preventDefault();
                    toggleFavoriteOrg(org.id, org.isFavorite ?? false);
                }}
                className="z-10"
                >
                {org.isFavorite ? (
                    <HiHeart className="w-5 h-5 text-red-600" />
                ) : (
                    <HiOutlineHeart className="w-5 h-5 text-black" />
                )}
                </button>
            </Link>
            ))}
          </div>
        )}
      </div>
    </div>
  );
};

export default FavoritesPage;

import { useEffect, useState } from "react";
import { useNavigate, useParams } from "react-router-dom";
import { API, StaticAPI } from "../services/api";
import { Announcement, Category } from "../types/ads";
import { useAuth } from "../context/AuthContext";
import { ChevronDown, ChevronUp } from "lucide-react";
import { AddressAutocomplete } from "../components/geo/AddressAutocomplete";

const AdFormPage = () => {
  const { announcementId } = useParams();
  const navigate = useNavigate();
  const { isAuth } = useAuth();

  useEffect(() => {
    if (!isAuth) {
      navigate("/login");
    }
  }, [isAuth, navigate]);

  const [title, setTitle] = useState("");
  const [description, setDescription] = useState("");
  const [categoryId, setCategoryId] = useState("");
  const [categories, setCategories] = useState<Category[]>([]);
  const [expandedCategories, setExpandedCategories] = useState<Set<string>>(new Set());
  const [region, setRegion] = useState("");
  const [city, setCity] = useState("");
  const [street, setStreet] = useState("");
  const [house, setHouse] = useState("");
  const [expirationDate, setExpirationDate] = useState("");
  const [imageFile, setImageFile] = useState<File | null>(null);
  const [preview, setPreview] = useState<string | null>(null);
  const [imagePath, setImagePath] = useState("");
  const [userId, setUserId] = useState<string | null>(null);
  const [showCategoryDropdown, setShowCategoryDropdown] = useState(false);
  const [message, setMessage] = useState<string | null>(null);
  const [messageType, setMessageType] = useState<"success" | "error">("success");

  const showTempMessage = (msg: string, type: "success" | "error") => {
    setMessage(msg);
    setMessageType(type);
    setTimeout(() => setMessage(null), 4000);
  };

  const isEditing = !!announcementId;

  useEffect(() => {
    API.get("/category").then((res) => {
      setCategories(res.data);
    });

    if (isAuth) {
      API.get("/user/me").then((res) => setUserId(res.data.userId));
    }

    if (announcementId) {
      API.get(`/announcement/${announcementId}`).then((res) => {
        const ad: Announcement = res.data;
        setTitle(ad.title);
        setDescription(ad.description || "");
        setRegion(ad.address?.region || "");
        setCity(ad.address?.city || "");
        setStreet(ad.address?.street || "");
        setHouse(ad.address?.house || "");
        setExpirationDate(ad.expirationDate?.split("T")[0] || "");
        setCategoryId(ad.category?.categoryId || "");
        setPreview(`${StaticAPI.defaults.baseURL}${ad.image}`);
        setImagePath(`${ad.image}`);
      });
    }
  }, [announcementId, isAuth]);

  const buildCategoryTree = (flatCategories: Category[]): Category[] => {
    const map = new Map(flatCategories.map((c) => [c.id, { ...c, children: [] as Category[] }]));
    const tree: Category[] = [];

    map.forEach((cat) => {
      if (cat.parentId) {
        map.get(cat.parentId)?.children?.push(cat);
      } else {
        tree.push(cat);
      }
    });

    return tree;
  };

  const categoryTree = buildCategoryTree(categories);

  const toggleCategory = (id: string) => {
    const updated = new Set(expandedCategories);
    updated.has(id) ? updated.delete(id) : updated.add(id);
    setExpandedCategories(updated);
  };

  const CategorySelector = ({ categories }: { categories: Category[] }) => (
    <div className="space-y-1">
      {categories.map((category) => (
        <div key={category.id} className="rounded-md overflow-hidden">
          <label
            className="flex items-center justify-between p-2 cursor-pointer font-medium text-black rounded-md"
            style={{ backgroundColor: category.color || "#4CAF50" }}
          >
            <div className="flex items-center gap-2">
              <input
                type="radio"
                name="category"
                value={category.id}
                checked={categoryId === category.id}
                onChange={() => setCategoryId(category.id)}
                className="accent-white"
              />
              <span>{category.name}</span>
            </div>
            <button type="button" onClick={() => toggleCategory(category.id)}>
              {expandedCategories.has(category.id) ? <ChevronUp size={16} /> : <ChevronDown size={16} />}
            </button>
          </label>
          {expandedCategories.has(category.id) && category.children && (
            <div className="pl-4 pt-1 space-y-1">
              {category.children.map((child) => (
                <label
                  key={child.id}
                  className="flex items-center space-x-2 p-2 rounded-md text-sm text-black cursor-pointer"
                  style={{ backgroundColor: child.color || "#81C784" }}
                >
                  <input
                    type="radio"
                    name="category"
                    value={child.id}
                    checked={categoryId === child.id}
                    onChange={() => setCategoryId(child.id)}
                    className="accent-white"
                  />
                  <span>{child.name}</span>
                </label>
              ))}
            </div>
          )}
        </div>
      ))}
    </div>
  );

  const handleFileChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const file = e.target.files?.[0];
    if (file) {
      setImageFile(file);
      setPreview(URL.createObjectURL(file));
    }
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();

    if (!imageFile && !isEditing) {
      alert("Загрузите изображение");
      return;
    }

    const formData = new FormData();
    if (imageFile) formData.append("ImageFile", imageFile);

    const params = {
      Title: title,
      Description: description,
      "Address.Region": region,
      "Address.City": city,
      "Address.Street": street,
      "Address.House": house,
      ExpirationDate: expirationDate,
      CategoryId: categoryId,
      UserId: userId!,
      ImagePath: imagePath,
      ...(isEditing && { Id: announcementId }),
    };

    Object.entries(params).forEach(([key, value]) => {
      formData.append(key, value.toString());
    });

    try {
      if (isEditing) {
        await API.put(`/announcement`, formData);
        navigate("/profile/ads");
      } else {
        await API.post(`/announcement`, formData);
        navigate("/profile/ads");
      }
    } catch (err: any) {
      const msg =
        typeof err?.response?.data === "string"
          ? err.response.data
          : "Ошибка при отправке объявления";
      showTempMessage(msg, "error");
    }
  };

  return (
    <div className="max-w-7xl mx-auto px-4 py-8">
      {message && (
        <div
          className={`fixed top-4 left-1/2 transform -translate-x-1/2 px-6 py-3 rounded-xl shadow-md z-50 animate-fade-in ${
            messageType === "success" ? "bg-green-500 text-white" : "bg-red-500 text-white"
          }`}
        >
          {message}
        </div>
      )}

      <div className="bg-white rounded-2xl shadow-md p-6 flex flex-col md:flex-row gap-6">
        <div className="w-full md:w-1/2">
          <label className="block text-sm font-medium text-gray-700 mb-2">Фото</label>
          <label className="block border-2 border-dashed rounded-xl cursor-pointer h-64 flex items-center justify-center overflow-hidden hover:border-primary transition-colors duration-200">
            {preview ? (
              <img src={preview} alt="Preview" className="object-cover w-full h-full rounded-xl" />
            ) : (
              <div className="text-gray-400">Нажмите для загрузки</div>
            )}
            <input type="file" onChange={handleFileChange} className="hidden" accept="image/*" />
          </label>
        </div>

        <form onSubmit={handleSubmit} className="w-full md:w-1/2 space-y-4">
          <input
            type="text"
            placeholder="Заголовок объявления"
            value={title}
            onChange={(e) => setTitle(e.target.value)}
            className="w-full border rounded-lg px-4 py-2"
            maxLength={50}
            required
          />

          <textarea
            placeholder="Описание (необязательно)"
            value={description}
            onChange={(e) => setDescription(e.target.value)}
            className="w-full border rounded-lg px-4 py-2"
            rows={3}
            maxLength={500}
          />

          {!isEditing && (
            <AddressAutocomplete
              onSelect={({ region, city, street, house }) => {
                setRegion(region);
                setCity(city);
                setStreet(street);
                setHouse(house);
              }}
              showMessage={showTempMessage}
            />
          )}


          <label className="block text-sm font-medium mb-2 text-gray-700">
            Дата истечения срока годности
          </label>
          <input
            type="date"
            value={expirationDate}
            onChange={(e) => setExpirationDate(e.target.value)}
            className="w-full border rounded-lg px-4 py-2"
            required
          />

          <div>
            <label className="block text-sm font-medium mb-2 text-gray-700">Категория</label>
            <div className="relative">
              <button
                type="button"
                className="w-full text-left border rounded-lg px-4 py-2 bg-gray-100"
                onClick={() => setShowCategoryDropdown(!showCategoryDropdown)}
              >
                {categoryId
                  ? categories.find((cat) => cat.id === categoryId)?.name || "Выбрать категорию"
                  : "Выбрать категорию"}
              </button>
              {showCategoryDropdown && (
                <div className="absolute z-10 w-full mt-2 border rounded-lg p-2 bg-white max-h-64 overflow-y-auto shadow-md">
                  <CategorySelector categories={categoryTree} />
                </div>
              )}
            </div>
          </div>

          <div className="flex gap-4">
            <button
              type="submit"
              className="w-1/2 bg-green-600 hover:bg-green-700 text-white py-3 px-6 rounded-xl font-medium transition-colors"
              disabled={!userId}
            >
              {isEditing ? "Сохранить" : "Создать"}
            </button>
            <button
              type="button"
              onClick={() => navigate(isEditing ? "/profile/ads" : "/ads")}
              className="w-1/2 bg-gray-200 hover:bg-gray-300 text-gray-800 py-3 px-6 rounded-xl font-medium transition-colors"
            >
              Отмена
            </button>
          </div>
        </form>
      </div>
    </div>
  );
};

export default AdFormPage;

//AdFormPage.tsx
import { useEffect, useState } from "react";
import { useNavigate, useParams } from "react-router-dom";
import { API } from "../services/api";
import { Announcement } from "../types/ads";
import { useAuth } from "../context/AuthContext";

const AdFormPage = () => {
  const { announcementId } = useParams();
  const navigate = useNavigate();
  const { isAuth } = useAuth();

  const [title, setTitle] = useState("");
  const [description, setDescription] = useState("");
  const [categoryId, setCategoryId] = useState("");
  const [categories, setCategories] = useState<{ id: string; name: string }[]>([]);
  const [region, setRegion] = useState("");
  const [city, setCity] = useState("");
  const [street, setStreet] = useState("");
  const [house, setHouse] = useState("");
  const [expirationDate, setExpirationDate] = useState("");
  const [imageFile, setImageFile] = useState<File | null>(null);
  const [preview, setPreview] = useState<string | null>(null);
  const [userId, setUserId] = useState<string | null>(null);

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
        setPreview(`${ad.image?.startsWith("/") ? "" : "/"}${ad.image}`);
      });
    }
  }, [announcementId, isAuth]);

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

    const params: Record<string, string> = {
      Title: title,
      Description: description,
      "Address.Region": region,
      "Address.City": city,
      "Address.Street": street,
      "Address.House": house,
      ExpirationDate: expirationDate,
      CategoryId: categoryId,
      UserId: userId!,
    };

    const queryString = new URLSearchParams(params).toString();

    try {
      if (isEditing) {
        // Обновление логики пока нет в схеме, но можно реализовать аналогично
        alert("Редактирование ещё не реализовано");
      } else {
        await API.post(`/announcement?${queryString}`, formData);
        navigate("/ads");
      }
    } catch (err) {
      console.error("Ошибка при отправке объявления", err);
    }
  };

  return (
    <div className="max-w-2xl mx-auto py-8">
      <h1 className="text-2xl font-bold mb-6">
        {isEditing ? "Редактировать объявление" : "Создать объявление"}
      </h1>
      <form onSubmit={handleSubmit} className="space-y-6">
        <div>
          <label className="block font-medium mb-1">Заголовок</label>
          <input
            type="text"
            value={title}
            onChange={(e) => setTitle(e.target.value)}
            className="w-full border rounded-xl px-4 py-2"
            maxLength={50}
            required
          />
        </div>

        <div>
          <label className="block font-medium mb-1">Описание</label>
          <textarea
            value={description}
            onChange={(e) => setDescription(e.target.value)}
            className="w-full border rounded-xl px-4 py-2"
            rows={3}
            maxLength={500}
          />
        </div>

        <div className="grid grid-cols-2 gap-4">
          <div>
            <label className="block font-medium mb-1">Регион</label>
            <input
              type="text"
              value={region}
              onChange={(e) => setRegion(e.target.value)}
              className="w-full border rounded-xl px-4 py-2"
            />
          </div>
          <div>
            <label className="block font-medium mb-1">Город</label>
            <input
              type="text"
              value={city}
              onChange={(e) => setCity(e.target.value)}
              className="w-full border rounded-xl px-4 py-2"
            />
          </div>
        </div>

        <div className="grid grid-cols-2 gap-4">
          <div>
            <label className="block font-medium mb-1">Улица</label>
            <input
              type="text"
              value={street}
              onChange={(e) => setStreet(e.target.value)}
              className="w-full border rounded-xl px-4 py-2"
            />
          </div>
          <div>
            <label className="block font-medium mb-1">Дом</label>
            <input
              type="text"
              value={house}
              onChange={(e) => setHouse(e.target.value)}
              className="w-full border rounded-xl px-4 py-2"
            />
          </div>
        </div>

        <div>
          <label className="block font-medium mb-1">Категория</label>
          <select
            value={categoryId}
            onChange={(e) => setCategoryId(e.target.value)}
            className="w-full border rounded px-4 py-2"
            required
          >
            <option value="">Выберите категорию</option>
            {categories.map((c) => (
              <option key={c.id} value={c.id}>
                {c.name}
              </option>
            ))}
          </select>
        </div>

        <div>
          <label className="block font-medium mb-1">Срок годности</label>
          <input
            type="date"
            value={expirationDate}
            onChange={(e) => setExpirationDate(e.target.value)}
            className="w-full border rounded px-4 py-2"
            required
          />
        </div>

        {/* Фото-поле с превью */}
        <div className="relative group">
          <label className="block mb-2 text-sm font-medium text-gray-700">
            Фото
          </label>
          <div className="flex items-center justify-center w-full">
            <label className="flex flex-col items-center justify-center w-full h-32 border-2 border-dashed rounded-xl cursor-pointer hover:border-primary transition-colors">
              {preview ? (
                <img
                  src={preview}
                  alt="Preview"
                  className="w-full h-full object-cover rounded-xl"
                />
              ) : (
                <>
                  <svg
                    className="w-8 h-8 text-gray-400 mb-2"
                    fill="none"
                    stroke="currentColor"
                    viewBox="0 0 24 24"
                  >
                    <path
                      strokeLinecap="round"
                      strokeLinejoin="round"
                      strokeWidth="2"
                      d="M4 16l4.586-4.586a2 2 0 012.828 0L16 16m-2-2l1.586-1.586a2 2 0 012.828 0L20 14m-6-6h.01M6 20h12a2 2 0 002-2V6a2 2 0 00-2-2H6a2 2 0 00-2 2v12a2 2 0 002 2z"
                    />
                  </svg>
                  <span className="text-sm text-gray-500">
                    Нажмите для загрузки
                  </span>
                </>
              )}
              <input
                type="file"
                onChange={handleFileChange}
                className="hidden"
                accept="image/*"
              />
            </label>
          </div>
        </div>

        <div>
          <button
            type="submit"
            className="bg-primary text-white px-6 py-3 rounded-lg hover:bg-green-600 transition-colors"
            disabled={!userId}
          >
            {isEditing ? "Сохранить изменения" : "Создать"}
          </button>
        </div>
      </form>
    </div>
  );
};

export default AdFormPage;

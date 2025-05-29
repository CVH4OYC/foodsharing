// src/pages/EditProfilePage.tsx
import { useEffect, useRef, useState } from "react";
import { API, StaticAPI } from "../services/api";
import { UserProfile } from "../types/ads";
import { useNavigate } from "react-router-dom";

const EditProfilePage = () => {
  const [profile, setProfile] = useState<UserProfile | null>(null);
  const [firstName, setFirstName] = useState("");
  const [lastName, setLastName] = useState("");
  const [bio, setBio] = useState("");
  const [image, setImage] = useState<File | null>(null);
  const [imagePreview, setImagePreview] = useState<string | null>(null);
  const [deleteImage, setDeleteImage] = useState(false);
  const fileInputRef = useRef<HTMLInputElement | null>(null);
  const navigate = useNavigate();

  useEffect(() => {
    const fetchProfile = async () => {
      try {
        const res = await API.get("/user/my");
        const data = res.data;
        setProfile(data);
        setFirstName(data.firstName || "");
        setLastName(data.lastName || "");
        setBio(data.bio || "");
        if (data.image) {
          setImagePreview(`${StaticAPI.defaults.baseURL}${data.image}`);
        }
      } catch (err) {
        console.error("Ошибка загрузки профиля", err);
      }
    };

    fetchProfile();
  }, []);

  const handleImageChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const file = e.target.files?.[0];
    if (file) {
      setImage(file);
      setImagePreview(URL.createObjectURL(file));
      setDeleteImage(false);
    }
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    if (!profile) return;

    const formData = new FormData();
    formData.append("UserId", profile.userId);
    formData.append("FirstName", firstName);
    formData.append("LastName", lastName);
    formData.append("Bio", bio);

    if (deleteImage) {
      // Ничего не добавляем — это будет интерпретироваться как удаление
    } else if (image) {
      formData.append("ImageFile", image);
    }

    try {
      await API.put("/user/profile", formData, {
        headers: { "Content-Type": "multipart/form-data" },
      });
      navigate("/profile/my");
    } catch (err) {
      console.error("Ошибка обновления профиля", err);
    }
  };

  return (
    <div className="py-8 pr-4">
      <h1 className="text-2xl font-bold mb-6">Редактировать профиль</h1>
      <form onSubmit={handleSubmit} className="space-y-4 max-w-xl">
        {/* Аватар */}
        <div>
          <label className="block text-sm font-medium text-gray-700 mb-1">Аватар</label>
          {imagePreview ? (
            <div className="relative w-24 h-24 mb-2">
              <img
                src={imagePreview}
                alt="Preview"
                className="w-24 h-24 object-cover rounded-full border"
              />
              <button
                type="button"
                onClick={() => {
                  setImage(null);
                  setImagePreview(null);
                  setDeleteImage(true);
                }}
                className="absolute -top-2 -right-2 bg-red-500 text-white rounded-full p-1 text-xs"
              >
                ✕
              </button>
            </div>
          ) : (
            <div className="w-24 h-24 bg-gray-200 rounded-full mb-2" />
          )}
          <input
            type="file"
            accept="image/*"
            ref={fileInputRef}
            onChange={handleImageChange}
            className="block text-sm text-gray-600"
          />
        </div>
  
        <div>
          <label className="block text-sm font-medium text-gray-700 mb-1">Имя</label>
          <input
            type="text"
            value={firstName}
            onChange={(e) => setFirstName(e.target.value)}
            required
            className="w-full border rounded-lg p-2"
          />
        </div>
  
        <div>
          <label className="block text-sm font-medium text-gray-700 mb-1">Фамилия</label>
          <input
            type="text"
            value={lastName}
            onChange={(e) => setLastName(e.target.value)}
            className="w-full border rounded-lg p-2"
          />
        </div>
  
        <div>
          <label className="block text-sm font-medium text-gray-700 mb-1">О себе</label>
          <textarea
            value={bio}
            onChange={(e) => setBio(e.target.value)}
            rows={4}
            className="w-full border rounded-lg p-2"
          />
        </div>
  
        <div className="flex gap-3">
          <button
            type="submit"
            className="bg-primary hover:bg-green-600 text-white py-2 px-4 rounded-xl font-medium"
          >
            Сохранить изменения
          </button>
          <button
            type="button"
            onClick={() => navigate("/profile/my")}
            className="py-2 px-4 rounded-xl border border-gray-300 text-gray-700 hover:bg-gray-100 font-medium"
          >
            Отмена
          </button>
        </div>

      </form>
    </div>
  );
  
};

export default EditProfilePage;

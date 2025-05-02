// src/pages/Register.tsx
import { useState } from "react";
import { Link, useNavigate } from "react-router-dom";
import { API } from "../services/api";
import { useAuth } from "../context/AuthContext"; // добавлено

const Register = () => {
  const [formData, setFormData] = useState({
    firstName: "",
    lastName: "",
    userName: "",
    password: "",
    bio: "",
  });
  const [imageFile, setImageFile] = useState<File | null>(null);
  const [preview, setPreview] = useState<string>("");
  const [error, setError] = useState("");
  const navigate = useNavigate();
  const { updateAuth } = useAuth(); // добавлено

  const handleFileChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const allowedExtensions = [".jpg", ".jpeg", ".png"];
    const file = e.target.files?.[0];

    if (file) {
      const extension = file.name.split(".").pop()?.toLowerCase();
      if (!allowedExtensions.includes(`.${extension}`)) {
        setError("Допустимые форматы: JPG, JPEG, PNG");
        return;
      }

      setImageFile(file);
      setPreview(URL.createObjectURL(file));
      setError("");
    }
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    try {
      const formDataToSend = new FormData();
      formDataToSend.append("FirstName", formData.firstName);
      formDataToSend.append("LastName", formData.lastName || "");
      formDataToSend.append("UserName", formData.userName);
      formDataToSend.append("Password", formData.password);
      formDataToSend.append("Bio", formData.bio || "");
      if (imageFile) formDataToSend.append("ImageFile", imageFile);

      const response = await API.post("/User/register", formDataToSend, {
        headers: { "Content-Type": "multipart/form-data" },
      });

      if (response.status === 200) {
        const loginRes = await API.post("/User/login", {
          userName: formData.userName,
          password: formData.password,
        });
        localStorage.setItem("token", loginRes.data.token);
        updateAuth(); // обновление авторизации
        navigate("/"); // переход на главную
      }
    } catch (err: any) {
      setError(err.response?.data || "Ошибка регистрации");
    }
  };

  return (
    <div className="min-h-screen flex items-center justify-center bg-white">
      <div className="w-full max-w-md p-6 bg-white rounded-xl shadow-lg">
        <h2 className="text-2xl font-bold text-center mb-6 text-primary">
          Регистрация
        </h2>

        {error && (
          <div className="mb-4 p-3 bg-red-100 text-red-700 rounded-lg">
            {error}
          </div>
        )}

        <form onSubmit={handleSubmit} className="space-y-4">
          <input
            type="text"
            placeholder="Имя *"
            value={formData.firstName}
            onChange={(e) =>
              setFormData({ ...formData, firstName: e.target.value })
            }
            className="w-full p-2 border rounded-xl focus:ring-2 focus:ring-primary"
            required
          />

          <input
            type="text"
            placeholder="Фамилия"
            value={formData.lastName}
            onChange={(e) =>
              setFormData({ ...formData, lastName: e.target.value })
            }
            className="w-full p-2 border rounded-xl focus:ring-2 focus:ring-primary"
          />

          <input
            type="text"
            placeholder="Имя пользователя *"
            value={formData.userName}
            onChange={(e) =>
              setFormData({ ...formData, userName: e.target.value })
            }
            className="w-full p-2 border rounded-xl focus:ring-2 focus:ring-primary"
            required
          />

          <input
            type="password"
            placeholder="Пароль *"
            value={formData.password}
            onChange={(e) =>
              setFormData({ ...formData, password: e.target.value })
            }
            className="w-full p-2 border rounded-xl focus:ring-2 focus:ring-primary"
            required
          />

          <input
            type="text"
            placeholder="О себе"
            value={formData.bio}
            onChange={(e) =>
              setFormData({ ...formData, bio: e.target.value })
            }
            className="w-full p-2 border rounded-xl focus:ring-2 focus:ring-primary"
          />

          <div className="relative group">
            <label className="block mb-2 text-sm font-medium text-gray-700">
              Фото профиля
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

          <button
            type="submit"
            className="w-full bg-primary text-white py-2 rounded-xl hover:opacity-90 transition-all"
          >
            Зарегистрироваться
          </button>
        </form>

        <div className="mt-4 text-center">
          <span className="text-gray-600">Уже есть аккаунт? </span>
          <Link to="/login" className="text-primary hover:underline">
            Войти
          </Link>
        </div>
      </div>
    </div>
  );
};

export default Register;

import { useState, useEffect, useRef } from "react";
import { Link, useNavigate } from "react-router-dom";
import { HiMenu, HiX, HiOutlineChat, HiOutlineHeart, HiOutlineUser } from "react-icons/hi";
import { API } from "../services/api";
import { useAuth } from "../context/AuthContext";
import CatalogMenu from "./CatalogMenu"; // 👉 импортируем меню

const Header = () => {
  const { isAuth, logout, hasRole } = useAuth();
  const [isMenuOpen, setIsMenuOpen] = useState(false);
  const [isCatalogOpen, setIsCatalogOpen] = useState(false);
  const menuRef = useRef<HTMLDivElement>(null);
  const catalogRef = useRef<HTMLDivElement>(null);
  const navigate = useNavigate();

  useEffect(() => {
    const handleClickOutside = (e: MouseEvent) => {
      if (
        menuRef.current &&
        !menuRef.current.contains(e.target as Node) &&
        catalogRef.current &&
        !catalogRef.current.contains(e.target as Node)
      ) {
        setIsMenuOpen(false);
        setIsCatalogOpen(false);
      }
    };
    document.addEventListener("mousedown", handleClickOutside);
    return () => document.removeEventListener("mousedown", handleClickOutside);
  }, []);

  const handleLogout = async () => {
    try {
      await API.post("/User/logout", {}, { withCredentials: true });
      logout();
      navigate("/");
    } catch (err) {
      console.error("Ошибка выхода:", err);
    }
  };

  const CatalogIcon = () => (
    <svg className="w-6 h-6 text-white" viewBox="0 0 24 24" fill="none">
      <rect x="3" y="3" width="8" height="8" rx="2" fill="currentColor" />
      <rect x="13" y="3" width="8" height="8" rx="2" fill="currentColor" />
      <rect x="3" y="13" width="8" height="8" rx="2" fill="currentColor" />
      <rect x="13" y="13" width="8" height="8" rx="2" fill="currentColor" />
    </svg>
  );

  return (
    <header className="sticky top-0 z-50 bg-white shadow-md">
      <div className="mx-auto px-4 sm:px-6 lg:px-8 max-w-7xl">
        <div className="flex items-center justify-between h-16">
          {/* Логотип и Каталог */}
          <div className="flex items-center gap-4">
            <Link to="/" className="text-xl font-bold text-primary">
              ЕДАМ
            </Link>
            <button
              onClick={() => setIsCatalogOpen(!isCatalogOpen)}
              className="hidden md:flex items-center gap-2 bg-primary text-white px-4 py-2 rounded-xl"
            >
              {isCatalogOpen ? <HiX className="w-6 h-6" /> : <CatalogIcon />}
              <span>Каталог</span>
            </button>
          </div>

          {/* Бургер-меню */}
          <button
            className="md:hidden p-2 text-primary"
            onClick={() => setIsMenuOpen(!isMenuOpen)}
          >
            {isMenuOpen ? <HiX className="h-6 w-6" /> : <HiMenu className="h-6 w-6" />}
          </button>

          {/* Навигация */}
          <nav className="hidden md:flex items-center gap-6">
            <Link to="/ads" className="hover:text-primary">Объявления</Link>
            {hasRole("Admin") ? (
              <Link to="/applications" className="hover:text-primary">Заявки на партнёрство</Link>
            ) : (
              <>
                <Link to="/business" className="hover:text-primary">Бизнесу</Link>
                <Link to="/about" className="hover:text-primary">О нас</Link>
              </>
            )}
          </nav>

          {/* Авторизация (desktop) */}
          <div className="hidden md:flex items-center gap-4">
            {isAuth ? (
              <>
                <Link to="/chats" className="hover:text-primary flex items-center gap-1">
                  <HiOutlineChat className="h-5 w-5" />
                  Чаты
                </Link>
                <Link to="/favorites" className="hover:text-primary flex items-center gap-1">
                  <HiOutlineHeart className="h-5 w-5" />
                  Избранное
                </Link>
                <Link to="/profile" className="hover:text-primary flex items-center gap-1">
                  <HiOutlineUser className="h-5 w-5" />
                  Профиль
                </Link>
              </>
            ) : (
              <>
                <Link to="/login" className="hover:text-primary">Войти</Link>
                <Link to="/register" className="bg-primary text-white px-4 py-2 rounded-xl hover:opacity-90">
                  Зарегистрироваться
                </Link>
              </>
            )}
          </div>
        </div>

        {/* Каталог (desktop) */}
        {isCatalogOpen && (
          <div ref={catalogRef}>
            <CatalogMenu />
          </div>
        )}

        {/* Мобильное меню */}
        {isMenuOpen && (
          <div
            ref={menuRef}
            className="md:hidden absolute bg-white w-full left-0 px-4 pb-4 shadow-lg z-30"
          >
            <div className="pt-2 space-y-4">
              {isAuth ? (
                <>
                  <Link to="/chats" className="block hover:text-primary">Чаты</Link>
                  <Link to="/favorites" className="block hover:text-primary">Избранное</Link>
                  <Link to="/profile" className="block hover:text-primary">Профиль</Link>
                  {hasRole("Admin") ? (
                    <Link to="/applications" className="block hover:text-primary">Заявки на партнёрство</Link>
                  ) : (
                    <>
                      <Link to="/business" className="block hover:text-primary">Бизнесу</Link>
                      <Link to="/about" className="block hover:text-primary">О нас</Link>
                    </>
                  )}
                </>
              ) : (
                <>
                  <Link to="/ads" className="block hover:text-primary">Объявления</Link>
                  <Link to="/business" className="block hover:text-primary">Бизнесу</Link>
                  <Link to="/about" className="block hover:text-primary">О нас</Link>
                </>
              )}

              <div className="pt-4 border-t">
                {isAuth ? (
                  <button
                    onClick={handleLogout}
                    className="block w-full text-left hover:text-primary py-2"
                  >
                    Выйти
                  </button>
                ) : (
                  <>
                    <Link to="/login" className="block py-2 hover:text-primary">Войти</Link>
                    <Link to="/register" className="block bg-primary text-white px-4 py-2 rounded-xl mt-2">
                      Зарегистрироваться
                    </Link>
                  </>
                )}
              </div>
            </div>
          </div>
        )}
      </div>
    </header>
  );
};

export default Header;

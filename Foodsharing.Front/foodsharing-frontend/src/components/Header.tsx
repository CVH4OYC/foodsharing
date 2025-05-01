// src/components/Header.tsx
import { useState, useEffect, useRef } from "react";
import { Link } from "react-router-dom";
import { HiMenu, HiX } from "react-icons/hi";

const Header = () => {
  const [isMenuOpen, setIsMenuOpen] = useState(false);
  const menuRef = useRef<HTMLDivElement>(null);

  const CatalogIcon = () => (
    <svg className="w-6 h-6 text-white" viewBox="0 0 24 24" fill="none">
      <rect x="3" y="3" width="8" height="8" rx="2" fill="currentColor"/>
      <rect x="13" y="3" width="8" height="8" rx="2" fill="currentColor"/>
      <rect x="3" y="13" width="8" height="8" rx="2" fill="currentColor"/>
      <rect x="13" y="13" width="8" height="8" rx="2" fill="currentColor"/>
    </svg>
  );

  // Закрытие меню при клике вне области
  useEffect(() => {
    const handleClickOutside = (event: MouseEvent) => {
      if (menuRef.current && !menuRef.current.contains(event.target as Node)) {
        setIsMenuOpen(false);
      }
    };

    document.addEventListener("mousedown", handleClickOutside);
    return () => document.removeEventListener("mousedown", handleClickOutside);
  }, []);

  return (
    <header className="bg-white shadow-md z-20 relative">
      <div className="mx-auto px-4 sm:px-6 lg:px-8 max-w-7xl">
        <div className="flex items-center justify-between h-16">
          {/* Левая часть */}
          <div className="flex items-center gap-4">
            <Link to="/" className="text-xl font-bold text-primary">
              ЕДАМ
            </Link>
            <button className="hidden md:flex items-center gap-2 bg-primary text-white px-4 py-2 rounded-xl">
              <CatalogIcon />
              <span>Каталог</span>
            </button>
          </div>

          {/* Бургер-меню */}
          <button 
            className="md:hidden p-2"
            onClick={() => setIsMenuOpen(!isMenuOpen)}
          >
            {isMenuOpen ? (
              <HiX className="h-6 w-6 text-gray-800" />
            ) : (
              <HiMenu className="h-6 w-6 text-gray-800" />
            )}
          </button>

          {/* Десктоп навигация */}
          <nav className="hidden md:flex items-center gap-6">
            <Link to="/ads" className="hover:text-primary">Объявления</Link>
            <Link to="/business" className="hover:text-primary">Бизнесу</Link>
            <Link to="/about" className="hover:text-primary">О нас</Link>
          </nav>

          {/* Кнопки авторизации */}
          <div className="hidden md:flex items-center gap-4">
            <Link to="/login" className="hover:text-primary">Войти</Link>
            <Link 
              to="/register" 
              className="bg-primary text-white px-4 py-2 rounded-xl hover:opacity-90"
            >
              Зарегистрироваться
            </Link>
          </div>
        </div>

        {/* Мобильное меню */}
        {isMenuOpen && (
          <div 
            ref={menuRef}
            className="md:hidden absolute bg-white w-full left-0 px-4 pb-4 shadow-lg z-30"
          >
            <div className="pt-2 space-y-4">
              <button className="w-full flex items-center gap-2 bg-primary text-white px-4 py-2 rounded-xl">
                <CatalogIcon />
                <span>Каталог</span>
              </button>
              
              <Link to="/ads" className="block hover:text-primary">Объявления</Link>
              <Link to="/business" className="block hover:text-primary">Бизнесу</Link>
              <Link to="/about" className="block hover:text-primary">О нас</Link>
              
              <div className="pt-4 border-t">
                <Link to="/login" className="block py-2 hover:text-primary">Войти</Link>
                <Link 
                  to="/register" 
                  className="block bg-primary text-white px-4 py-2 rounded-xl mt-2"
                >
                  Зарегистрироваться
                </Link>
              </div>
            </div>
          </div>
        )}
      </div>
    </header>
  );
};

export default Header;
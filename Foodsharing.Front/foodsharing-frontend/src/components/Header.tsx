// src/components/Header.tsx
import { Link } from "react-router-dom";

const Header = () => {
  // Иконка каталога (4 квадрата)
  const CatalogIcon = () => (
    <svg 
      className="w-6 h-6 text-white" 
      viewBox="0 0 24 24" 
      fill="none" 
      xmlns="http://www.w3.org/2000/svg"
    >
      <rect x="3" y="3" width="8" height="8" rx="2" fill="currentColor"/>
      <rect x="13" y="3" width="8" height="8" rx="2" fill="currentColor"/>
      <rect x="3" y="13" width="8" height="8" rx="2" fill="currentColor"/>
      <rect x="13" y="13" width="8" height="8" rx="2" fill="currentColor"/>
    </svg>
  );

  return (
    <header className="bg-white shadow-md h-[100px] w-full fixed top-0 z-50">
      {/* Внутренний контейнер с отступами */}
      <div className="mx-auto px-[240px] w-full h-full">
        <div className="max-w-[1440px] mx-auto h-full flex items-center justify-between">
          {/* Левая часть: лого + каталог */}
          <div className="flex items-center gap-6">
            <Link to="/" className="text-2xl font-bold text-[#4CAF50]">
              ЕДАМ
            </Link>
            <button className="bg-[#4CAF50] flex items-center gap-2 px-4 py-3 rounded-2xl hover:opacity-90">
              <CatalogIcon />
              <span className="text-white">Каталог</span>
            </button>
          </div>

          {/* Центральная навигация */}
          <nav className="flex gap-8">
            <Link to="/ads" className="hover:text-[#4CAF50] transition-colors">
              Объявления
            </Link>
            <Link to="/business" className="hover:text-[#4CAF50] transition-colors">
              Бизнесу
            </Link>
            <Link to="/about" className="hover:text-[#4CAF50] transition-colors">
              О нас
            </Link>
          </nav>

          {/* Правая часть: авторизация */}
          <div className="flex items-center gap-6">
            <Link 
              to="/login" 
              className="hover:text-[#4CAF50] transition-colors"
            >
              Войти
            </Link>
            <Link 
              to="/register" 
              className="bg-[#4CAF50] text-white px-6 py-3 rounded-2xl hover:opacity-90 transition-all"
            >
              Зарегистрироваться
            </Link>
          </div>
        </div>
      </div>
    </header>
  );
};

export default Header;
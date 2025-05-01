import { Link } from "react-router-dom";

const Header = () => {
  return (
    <header className="bg-white shadow-md">
      <nav className="container mx-auto px-4 py-3 flex items-center justify-between">
        <Link to="/" className="text-2xl font-bold text-blue-600">
          ЕДАМ
        </Link>
        
        <div className="hidden md:flex space-x-6">
          <Link to="/ads" className="hover:text-blue-600">Объявления</Link>
          <Link to="/business" className="hover:text-blue-600">Бизнесу</Link>
          <Link to="/about" className="hover:text-blue-600">О нас</Link>
        </div>

        <div className="flex items-center space-x-4">
          <Link to="/login" className="hover:text-blue-600">Войти</Link>
          <Link 
            to="/register" 
            className="bg-blue-500 text-white px-4 py-2 rounded hover:bg-blue-600"
          >
            Зарегистрироваться
          </Link>
        </div>
      </nav>
    </header>
  );
};

export default Header;
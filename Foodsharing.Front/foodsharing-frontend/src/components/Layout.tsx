// src/components/Layout.tsx
import { Outlet } from "react-router-dom";
import Header from "./Header";
import Footer from "./Footer";

const Layout = () => {
  return (
    <div className="min-w-[1920px] w-full">
      {/* Фиксированная шапка */}
      <Header />
      
      {/* Основной контент С ОТСТУПАМИ */}
      <div className="mx-auto px-[240px] w-full">
        <div className="max-w-[1440px] mx-auto flex flex-col min-h-screen">
          <main className="flex-grow pt-[100px]">
            <Outlet />
          </main>
        </div>
      </div>

      {/* Футер БЕЗ ОТСТУПОВ */}
      <Footer />
    </div>
  );
};

export default Layout;
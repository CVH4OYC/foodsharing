// src/components/Footer.tsx
const Footer = () => {
    return (
        <footer className="bg-[#F2FCE2] mt-auto">
        <div className="mx-auto px-4 sm:px-6 lg:px-8 max-w-7xl">
          <div className="grid grid-cols-1 md:grid-cols-3 gap-8 py-12">
            {/* Навигация */}
            <div>
              <h3 className="text-lg font-bold mb-4">Навигация</h3>
              <ul className="space-y-2">
                <li>
                  <a href="/ads" className="hover:text-primary transition-colors">
                    Объявления
                  </a>
                </li>
                <li>
                  <a href="/business" className="hover:text-primary transition-colors">
                    Для бизнеса
                  </a>
                </li>
              </ul>
            </div>
  
            {/* Компания */}
            <div>
              <h3 className="text-lg font-bold mb-4">Компания</h3>
              <ul className="space-y-2">
                <li>
                  <a href="/about" className="hover:text-primary transition-colors">
                    О нас
                  </a>
                </li>
                <li>
                  <a href="/contacts" className="hover:text-primary transition-colors">
                    Контакты
                  </a>
                </li>
                <li>
                  <a href="/jobs" className="hover:text-primary transition-colors">
                    Вакансии
                  </a>
                </li>
                <li>
                  <a href="/partners" className="hover:text-primary transition-colors">
                    Партнёры
                  </a>
                </li>
              </ul>
            </div>
  
            {/* Контакты */}
            <div>
              <h3 className="text-lg font-bold mb-4">Контакты</h3>
              <div className="space-y-2">
                <p className="text-gray-600">edam@mail.ru</p>
                <p className="text-gray-600">+7 (800) 555-35-35</p>
                <p className="mt-4 text-sm text-gray-500">
                  Присоединяйтесь к нашему сообществу
                </p>
              </div>
            </div>
          </div>
  
          <div className="border-t pt-6 mt-8 text-center text-gray-600">
            <p>© ЕДАМ 2025</p>
          </div>
        </div>
      </footer>
    );
  };
  
  export default Footer;
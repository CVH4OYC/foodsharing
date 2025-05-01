// src/components/Footer.tsx
const Footer = () => {
    return (
        <footer className="bg-[#F2FCE2] h-[490px] w-full">
        <div className="mx-auto px-[240px] w-full h-full">
          <div className="max-w-[1440px] mx-auto py-12">
            <div className="grid grid-cols-3 gap-8">
              <div>
                <h3 className="font-bold mb-4 text-lg">Навигация</h3>
                <ul className="space-y-3">
                  <li>
                    <a href="/ads" className="hover:text-[#4CAF50]">
                      Объявления
                    </a>
                  </li>
                  <li>
                    <a href="/business" className="hover:text-[#4CAF50]">
                      Для бизнеса
                    </a>
                  </li>
                </ul>
              </div>
  
              <div>
                <h3 className="font-bold mb-4 text-lg">Компания</h3>
                <ul className="space-y-3">
                  <li>
                    <a href="/about" className="hover:text-[#4CAF50]">
                      О нас
                    </a>
                  </li>
                  <li>
                    <a href="/contacts" className="hover:text-[#4CAF50]">
                      Контакты
                    </a>
                  </li>
                  <li>
                    <a href="/jobs" className="hover:text-[#4CAF50]">
                      Вакансии
                    </a>
                  </li>
                  <li>
                    <a href="/partners" className="hover:text-[#4CAF50]">
                      Партнёры
                    </a>
                  </li>
                </ul>
              </div>
  
              <div>
                <h3 className="font-bold mb-4 text-lg">Контакты</h3>
                <div className="space-y-3">
                  <p>edam@mail.ru</p>
                  <p>+7 (800) 555-35-35</p>
                  <p className="mt-4 text-sm text-gray-600">
                    Присоединяйтесь к нашему сообществу и следите за нами в соцсетях
                  </p>
                </div>
              </div>
            </div>
  
            <div className="mt-12 pt-8 border-t border-gray-300 text-center">
              <p className="text-gray-600">© ЕДАМ 2025</p>
            </div>
          </div>
        </div>
      </footer>
    );
  };
  
  export default Footer;
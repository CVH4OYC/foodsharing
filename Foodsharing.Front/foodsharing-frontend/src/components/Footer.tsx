// src/components/Footer.tsx
const Footer = () => {
    return (
      <footer className="bg-gray-100 mt-12">
        <div className="container mx-auto px-4 py-8">
          <div className="grid grid-cols-1 md:grid-cols-3 gap-8">
            <div>
              <h3 className="font-bold mb-4">Навигация</h3>
              <ul className="space-y-2">
                <li><a href="/ads" className="hover:text-blue-600">Объявления</a></li>
                <li><a href="/business" className="hover:text-blue-600">Для бизнеса</a></li>
              </ul>
            </div>
            
            <div>
              <h3 className="font-bold mb-4">Компания</h3>
              <ul className="space-y-2">
                <li><a href="/about" className="hover:text-blue-600">О нас</a></li>
                <li><a href="/contacts" className="hover:text-blue-600">Контакты</a></li>
                <li><a href="/jobs" className="hover:text-blue-600">Вакансии</a></li>
                <li><a href="/partners" className="hover:text-blue-600">Партнёры</a></li>
              </ul>
            </div>
  
            <div>
              <h3 className="font-bold mb-4">Контакты</h3>
              <p className="mb-2">edam@mail.ru</p>
              <p>+7 (800) 555-35-35</p>
              <div className="mt-4">
                <p className="text-sm">Присоединяйтесь к нашему сообществу и следите за нами в соцсетях</p>
              </div>
            </div>
          </div>
  
          <div className="mt-8 pt-6 border-t border-gray-300 text-center">
            <p>© ЕДАМ 2025</p>
          </div>
        </div>
      </footer>
    );
  };
  
  export default Footer;
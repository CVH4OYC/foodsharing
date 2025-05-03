import { useEffect, useState } from "react";
import { useParams, Link } from "react-router-dom";
import { API, StaticAPI } from "../services/api";
import { Announcement } from "../types/ads";

const AdPage = () => {
  const { announcementId } = useParams();
  const [ad, setAd] = useState<Announcement | null>(null);
  const [loading, setLoading] = useState(true);
  const [showMenu, setShowMenu] = useState(false);

  useEffect(() => {
    const fetchAd = async () => {
      try {
        const response = await API.get(`/Announcement/${announcementId}`);
        setAd(response.data);
      } catch (error) {
        console.error('Ошибка загрузки объявления:', error);
      } finally {
        setLoading(false);
      }
    };

    if (announcementId) fetchAd();
  }, [announcementId]);

  if (loading) return <div className="text-center py-8">Загрузка...</div>;
  if (!ad) return <div className="text-center py-8">Объявление не найдено</div>;

  return (
    <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 py-8">
      <div className="bg-white rounded-xl shadow-lg p-6">
        {/* Шапка */}
        <div className="flex justify-between items-start mb-6">
          <Link 
            to="/ads" 
            className="text-primary hover:text-primary-dark flex items-center gap-2"
          >
            <svg className="w-4 h-4" fill="none" stroke="currentColor" viewBox="0 0 24 24">
              <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M10 19l-7-7m0 0l7-7m-7 7h18" />
            </svg>
            Назад к списку
          </Link>
          
          <div className="relative">
            <button 
              onClick={() => setShowMenu(!showMenu)}
              className="hover:bg-gray-100 rounded-full p-2"
            >
              <svg className="w-5 h-5" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M12 5v.01M12 12v.01M12 19v.01M12 6a1 1 0 110-2 1 1 0 010 2zm0 7a1 1 0 110-2 1 1 0 010 2zm0 7a1 1 0 110-2 1 1 0 010 2z" />
              </svg>
            </button>
            
            {showMenu && (
              <div className="absolute right-0 mt-2 w-48 bg-white rounded-md shadow-lg py-1 border">
                <button
                  onClick={() => console.log('Редактировать')}
                  className="block w-full px-4 py-2 text-sm hover:bg-gray-100 text-left"
                >
                  Редактировать
                </button>
                <button
                  onClick={() => console.log('Удалить')}
                  className="block w-full px-4 py-2 text-sm hover:bg-gray-100 text-left text-red-600"
                >
                  Удалить
                </button>
              </div>
            )}
          </div>
        </div>

        {/* Основной контент */}
        <div className="grid md:grid-cols-2 gap-8">
          <div className="relative h-96 rounded-xl overflow-hidden">
            <img
              src={`${StaticAPI.defaults.baseURL}${ad.image}`}
              alt={ad.title}
              className="w-full h-full object-cover"
              onError={(e) => {
                (e.target as HTMLImageElement).src = "/placeholder-image.jpg";
              }}
            />
          </div>

          <div className="space-y-6">
            <div>
              <h1 className="text-3xl font-bold mb-4">{ad.title}</h1>
              <div className="flex flex-wrap gap-4 mb-6">
                <span 
                  className="px-3 py-1 rounded-full text-sm"
                  style={{ backgroundColor: ad.category?.color || '#4CAF50' }}
                >
                  {ad.category?.name}
                </span>
                <div className="flex items-center gap-2 text-gray-600">
                  <svg className="w-5 h-5" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                    <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M17.657 16.657L13.414 20.9a1.998 1.998 0 01-2.827 0l-4.244-4.243a8 8 0 1111.314 0z" />
                    <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M15 11a3 3 0 11-6 0 3 3 0 016 0z" />
                  </svg>
                  <span>
                    {[ad.address?.city, ad.address?.street, ad.address?.house]
                      .filter(Boolean)
                      .join(', ')}
                  </span>
                </div>
              </div>

              <div className="grid grid-cols-2 gap-4 bg-gray-50 p-4 rounded-lg mb-6">
                <div>
                  <p className="text-sm text-gray-500 mb-1">Срок годности</p>
                  <p className="font-medium">
                    {ad.expirationDate 
                      ? new Date(ad.expirationDate).toLocaleDateString('ru-RU')
                      : 'Не указано'}
                  </p>
                </div>
                <div>
                  <p className="text-sm text-gray-500 mb-1">Опубликовано</p>
                  <p className="font-medium">
                    {new Date(ad.dateCreation).toLocaleDateString('ru-RU')}
                  </p>
                </div>
              </div>

              <div className="mb-6">
                <h2 className="text-lg font-semibold mb-2">Описание</h2>
                <p className="text-gray-600 whitespace-pre-line">{ad.description}</p>
              </div>
            </div>

            <div className="border-t pt-6">
              <div className="flex items-center gap-4 mb-4">
                {ad.user.image ? (
                  <img
                    src={`${StaticAPI.defaults.baseURL}${ad.user.image}`}
                    alt={ad.user.userName}
                    className="w-12 h-12 rounded-full object-cover"
                  />
                ) : (
                  <div className="w-12 h-12 rounded-full bg-gray-200 flex items-center justify-center">
                    <span className="font-medium">
                      {ad.user.userName?.[0]?.toUpperCase() || 'U'}
                    </span>
                  </div>
                )}
                <div>
                  <p className="font-medium">{ad.user.userName}</p>
                  <div className="flex items-center gap-2">
                    <span className="text-yellow-500">★ 5,0</span>
                    <span className="text-sm text-gray-500">Автор объявления</span>
                  </div>
                </div>
              </div>

              <button
                className="w-full bg-green-500 hover:bg-green-600 text-white py-3 px-6 rounded-lg font-medium transition-colors"
                onClick={() => console.log('Бронирование...')}
              >
                Забронировать
              </button>
            </div>
          </div>
        </div>
      </div>
    </div>
  );
};

export default AdPage;
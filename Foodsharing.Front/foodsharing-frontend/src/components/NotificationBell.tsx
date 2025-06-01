import { useEffect, useRef, useState } from "react";
import { API } from "../services/api";
import { HiBell, HiOutlineX } from "react-icons/hi";
import { useNavigate } from "react-router-dom";

const NotificationBell = () => {
  const [notifications, setNotifications] = useState<any[]>([]);
  const [isOpen, setIsOpen] = useState(false);
  const [hasUnread, setHasUnread] = useState(false);
  const boxRef = useRef<HTMLDivElement>(null);
  const navigate = useNavigate();

  const fetchNotifications = async () => {
    try {
      const { data } = await API.get("/Notification", { withCredentials: true });
      setNotifications(data);
      setHasUnread(data.some((n: any) => n.status !== "Прочитано"));
    } catch (err) {
      console.error("Ошибка при загрузке уведомлений", err);
    }
  };

  const handleNotificationClick = async (id: string, adId: string) => {
    try {
      setIsOpen(false); // 🔒 Закрыть окно
      await API.put(`/Notification/${id}/read`, {}, { withCredentials: true });
      await fetchNotifications(); // обновляем список
      navigate(`/ads/${adId}`);
    } catch (err) {
      console.error("Ошибка при переходе по уведомлению", err);
    }
  };

  const handleClickOutside = (e: MouseEvent) => {
    if (boxRef.current && !boxRef.current.contains(e.target as Node)) {
      setIsOpen(false);
    }
  };

  useEffect(() => {
    fetchNotifications();
    const interval = setInterval(fetchNotifications, 30_000);
    return () => clearInterval(interval);
  }, []);

  useEffect(() => {
    if (isOpen) fetchNotifications();
  }, [isOpen]);

  useEffect(() => {
    document.addEventListener("mousedown", handleClickOutside);
    return () => document.removeEventListener("mousedown", handleClickOutside);
  }, []);

  return (
    <div className="relative flex items-center" ref={boxRef}>
      <button onClick={() => setIsOpen(!isOpen)} className="relative">
        <HiBell className="h-6 w-6 text-primary" />
        {hasUnread && (
          <span className="absolute top-0 right-0 block h-2 w-2 rounded-full bg-red-500"></span>
        )}
      </button>

      {isOpen && (
       <div className="absolute top-full right-0 mt-2 w-80 max-h-96 overflow-y-auto bg-white border border-gray-300 rounded-xl shadow-xl z-50">
          <div className="flex justify-between items-center px-4 py-2 border-b">
            <h3 className="text-lg font-semibold">Уведомления</h3>
            <button onClick={() => setIsOpen(false)}>
              <HiOutlineX className="w-5 h-5" />
            </button>
          </div>
          {notifications.length === 0 ? (
            <p className="p-4 text-gray-500">Нет уведомлений</p>
          ) : (
            <ul className="divide-y divide-gray-200">
              {notifications.map((n) => (
                <li
                  key={n.id}
                  className={`p-4 hover:bg-gray-50 cursor-pointer ${
                    n.status !== "Прочитано" ? "bg-gray-100" : ""
                  }`}
                  onClick={() => handleNotificationClick(n.id, n.announcementId)}
                >
                  {n.message}
                </li>
              ))}
            </ul>
          )}
        </div>
      )}
    </div>
  );
};

export default NotificationBell;

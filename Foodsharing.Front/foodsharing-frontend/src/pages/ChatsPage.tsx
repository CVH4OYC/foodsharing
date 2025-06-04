import { useEffect, useState, useCallback } from "react";
import { API } from "../services/api";
import ChatList from "../components/chat/ChatList";
import ChatWindowPlaceholder from "../components/chat/ChatWindowPlaceholder";
import { ChatDTO } from "../types/chat";
import { Outlet, useParams, useNavigate } from "react-router-dom";
import connection, { startConnection } from "../services/signalr-chat";

const ChatsPage = () => {
  const [chats, setChats] = useState<ChatDTO[]>([]);
  const [loading, setLoading] = useState(true);
  const { chatId } = useParams();
  const navigate = useNavigate();

  const fetchChats = useCallback(async () => {
    try {
      setLoading(true);
      const res = await API.get<ChatDTO[]>("/chat/my");
      // Сортировка по дате последнего сообщения (самые свежие — в начале)
      const sorted = [...res.data].sort((a, b) => {
        const dateA = a.message?.date ? new Date(a.message.date).getTime() : 0;
        const dateB = b.message?.date ? new Date(b.message.date).getTime() : 0;
        return dateB - dateA;
      });
      setChats(sorted);
    } catch (err) {
      console.error("Ошибка при загрузке списка чатов", err);
    } finally {
      setLoading(false);
    }
  }, []);

  useEffect(() => {
    fetchChats();
  }, [fetchChats]);

  // ─── Подписка на SignalR: ChatListUpdate ─────────────────────────────────────────────
  useEffect(() => {
    // 1) Запускаем соединение, если ещё не подключено
    startConnection()
      .then(() => {
        // 2) Подписываемся на приходящие апдейты по всем чатам
        connection.on(
          "ChatListUpdate",
          (updatedChat: {
            id: string;
            interlocutor: any;
            message: any;
            unreadCount: number;
          }) => {
            setChats((prev) => {
              // Проверяем, есть ли уже чат с таким id
              const idx = prev.findIndex((c) => c.id === updatedChat.id);
              let newChats = [...prev];
              if (idx >= 0) {
                // Заменяем старый объект новым (оставляем тип ChatDTO)
                newChats[idx] = {
                  id: updatedChat.id,
                  interlocutor: updatedChat.interlocutor,
                  message: updatedChat.message,
                  unreadCount: updatedChat.unreadCount,
                };
              } else {
                // Чат новый (вдруг), добавляем в начало
                newChats.unshift({
                  id: updatedChat.id,
                  interlocutor: updatedChat.interlocutor,
                  message: updatedChat.message,
                  unreadCount: updatedChat.unreadCount,
                });
              }
              // 3) Ресортируем снова по дате последнего сообщения
              newChats = newChats.sort((a, b) => {
                const dateA = a.message?.date ? new Date(a.message.date).getTime() : 0;
                const dateB = b.message?.date ? new Date(b.message.date).getTime() : 0;
                return dateB - dateA;
              });
              return newChats;
            });
          }
        );
      })
      .catch((e) => console.warn("Ошибка SignalR (ChatsPage)", e));

    // 4) В `cleanup` снимаем подписку, чтобы не было утечек
    return () => {
      connection.off("ChatListUpdate");
    };
  }, []);
  // ─────────────────────────────────────────────────────────────────────────────────────

  const handleSelectChat = (id: string) => {
    navigate(`/chats/${id}`);
  };

  return (
    <div className="max-w-7xl mx-auto flex border rounded-xl overflow-hidden shadow-md h-[80vh]">
      <ChatList
        chats={chats}
        loading={loading}
        selectedChatId={chatId || null}
        onSelectChat={handleSelectChat}
      />
      <Outlet context={{ onNewChatCreated: fetchChats }} />
    </div>
  );
};

export default ChatsPage;

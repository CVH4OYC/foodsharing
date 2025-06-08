import { useEffect, useState, useCallback } from "react";
import { API } from "../services/api";
import ChatList from "../components/chat/ChatList";
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

  // Подписываемся на обновления списка
  useEffect(() => {
    startConnection()
      .then(() => {
        connection.on("ChatListUpdate", () => {
          fetchChats();
        });
      })
      .catch((e) => console.warn("Ошибка SignalR (ChatsPage)", e));

    return () => {
      connection.off("ChatListUpdate");
    };
  }, [fetchChats]);

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
      <Outlet context={{
        onNewChatCreated: fetchChats,
        updateChatLocally: (updatedChat: ChatDTO) => {
          setChats((prevChats) => {
            const updated = prevChats.filter(c => c.id !== updatedChat.id);
            return [updatedChat, ...updated];
          });
        }
      }} />


    </div>
  );
};

export default ChatsPage;

import { useEffect, useState, useCallback } from "react";
import { API } from "../services/api";
import ChatList from "../components/chat/ChatList";
import ChatWindowPlaceholder from "../components/chat/ChatWindowPlaceholder";
import { ChatDTO } from "../types/chat";
import { Outlet, useParams, useNavigate } from "react-router-dom";

const ChatsPage = () => {
  const [chats, setChats] = useState<ChatDTO[]>([]);
  const [loading, setLoading] = useState(true);
  const { chatId } = useParams();
  const navigate = useNavigate();

  const fetchChats = useCallback(async () => {
    try {
      const res = await API.get("/chat/my");
      setChats(res.data);
    } catch (err) {
      console.error("Ошибка загрузки чатов", err);
    } finally {
      setLoading(false);
    }
  }, []);

  useEffect(() => {
    fetchChats();
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
      <Outlet context={{ onNewChatCreated: fetchChats }} />
    </div>
  );
};

export default ChatsPage;

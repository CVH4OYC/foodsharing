import { useEffect, useState } from "react";
import { API, StaticAPI } from "../services/api";
import ChatList from "../components/chat/ChatList";
import ChatWindowPlaceholder from "../components/chat/ChatWindowPlaceholder";
import { ChatDTO } from "../types/chat";

const ChatsPage = () => {
  const [chats, setChats] = useState<ChatDTO[]>([]);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    const fetchChats = async () => {
      try {
        const res = await API.get("/chat/my");
        setChats(res.data);
      } catch (err) {
        console.error("Ошибка загрузки чатов", err);
      } finally {
        setLoading(false);
      }
    };
    fetchChats();
  }, []);

  return (
    <div className="max-w-7xl mx-auto flex border rounded-xl overflow-hidden shadow-md h-[80vh]">
      <ChatList chats={chats} loading={loading} />
      <ChatWindowPlaceholder />
    </div>
  );
};

export default ChatsPage;

// src/pages/ChatWindowPage.tsx
import { useParams } from "react-router-dom";
import ChatWindow from "../components/chat/ChatWindow";

const ChatWindowPage = () => {
  const { chatId } = useParams();

  if (!chatId) return <div className="p-4">Чат не найден</div>;

  return <ChatWindow chatId={chatId} />;
};

export default ChatWindowPage;

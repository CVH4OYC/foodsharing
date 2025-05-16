// src/pages/ChatWindowPage.tsx
import { useParams, useSearchParams } from "react-router-dom";
import ChatWindow from "../components/chat/ChatWindow";

const ChatWindowPage = () => {
  const { chatId } = useParams();
  const [searchParams] = useSearchParams();
  const userId = searchParams.get("userId");

  if (!chatId && !userId) {
    return <div className="p-4">Чат не найден</div>;
  }

  return <ChatWindow chatId={chatId ?? null} interlocutorId={userId ?? undefined} />;
};

export default ChatWindowPage;

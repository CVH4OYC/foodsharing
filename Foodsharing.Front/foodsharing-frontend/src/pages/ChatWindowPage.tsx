import { useEffect, useState } from "react";
import { useParams, useSearchParams } from "react-router-dom";
import ChatWindow from "../components/chat/ChatWindow";

const ChatWindowPage = () => {
  const { chatId } = useParams();
  const [searchParams] = useSearchParams();
  const [loadedChatId, setLoadedChatId] = useState<string | null>(null);

  const userId = searchParams.get("userId"); 

  useEffect(() => {
    if (chatId) {
      setLoadedChatId(chatId);
    }
  }, [chatId]);

  return (
    <div className="flex-1 flex flex-col">
      {loadedChatId || userId ? (
        <ChatWindow chatId={loadedChatId} interlocutorId={userId || undefined} />
      ) : (
        <div className="flex-1 flex items-center justify-center text-gray-400">
          Выберите чат слева
        </div>
      )}
    </div>
  );
};

export default ChatWindowPage;

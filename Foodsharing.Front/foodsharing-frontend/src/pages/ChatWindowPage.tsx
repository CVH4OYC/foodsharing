import { useEffect, useState } from "react";
import { useParams } from "react-router-dom";
import ChatWindow from "../components/chat/ChatWindow";

const ChatWindowPage = () => {
  const { chatId } = useParams();
  const [loadedChatId, setLoadedChatId] = useState<string | null>(null);

  useEffect(() => {
    if (chatId) setLoadedChatId(chatId);
  }, [chatId]);

  return (

    <div className="flex-1 flex flex-col">
      {loadedChatId ? (
        <ChatWindow chatId={loadedChatId} />
      ) : (
        <div className="flex-1 flex items-center justify-center text-gray-400">
          Выберите чат слева
        </div>
      )}
    </div>
  );
};

export default ChatWindowPage;

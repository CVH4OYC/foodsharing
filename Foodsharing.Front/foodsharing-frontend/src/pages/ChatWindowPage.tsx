// src/pages/ChatWindowPage.tsx
import { useEffect, useState } from "react";
import { useParams, useSearchParams, useOutletContext } from "react-router-dom";
import ChatWindow from "../components/chat/ChatWindow";
import type { ChatDTO } from "../types/chat";

interface OutletContext {
  onNewChatCreated: () => void;
}

const ChatWindowPage = () => {
  const { chatId } = useParams<{ chatId: string }>();
  const [searchParams] = useSearchParams();
  const [loadedChatId, setLoadedChatId] = useState<string | null>(null);

  // Получаем колбэк из ChatsPage
  const { onNewChatCreated } = useOutletContext<OutletContext>();

  const userId = searchParams.get("userId") || undefined;

  useEffect(() => {
    if (chatId) {
      setLoadedChatId(chatId);
    }
  }, [chatId]);

  return (
    <div className="flex-1 flex flex-col">
      {loadedChatId || userId ? (
        <ChatWindow
          chatId={loadedChatId}
          interlocutorId={userId}
          onNewChatCreated={onNewChatCreated}
        />
      ) : (
        <div className="flex-1 flex items-center justify-center text-gray-400">
          Выберите чат слева
        </div>
      )}
    </div>
  );
};

export default ChatWindowPage;

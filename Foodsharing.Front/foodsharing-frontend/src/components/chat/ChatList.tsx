// src/components/chat/ChatList.tsx
import React from "react";
import { ChatDTO } from "../../types/chat";
import ChatListItem from "./ChatListItem";

interface Props {
  chats: ChatDTO[];
  loading: boolean;
  selectedChatId: string | null;
  onSelectChat: (chatId: string) => void;
}

const ChatList: React.FC<Props> = ({
  chats,
  loading,
  selectedChatId,
  onSelectChat,
}) => {
  return (
    <aside className="w-[300px] border-r overflow-y-auto">
      <h2 className="text-lg font-bold p-4">Чаты</h2>

      {chats.length > 0 ? (
        // Всегда показываем список, если есть хотя бы один чат
        <ul>
          {chats.map((chat) => (
            <ChatListItem
              key={chat.id}
              chat={chat}
              selected={chat.id === selectedChatId}
              onSelect={() => onSelectChat(chat.id)}
            />
          ))}
        </ul>
      ) : loading ? (
        // Показываем «Загрузка...» только если список пока пуст и идёт первый fetch
        <div className="p-4 text-gray-500">Загрузка...</div>
      ) : (
        // Если не loading и нет чатов
        <div className="p-4 text-gray-500">Нет чатов</div>
      )}
    </aside>
  );
};

export default ChatList;

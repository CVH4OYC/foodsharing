import { FC } from "react";
import { ChatDTO } from "../../types/chat";
import ChatListItem from "./ChatListItem";

interface Props {
  chats: ChatDTO[];
  loading: boolean;
  selectedChatId: string | null;
  onSelectChat: (chatId: string) => void;
}

const ChatList: FC<Props> = ({ chats, loading, selectedChatId, onSelectChat }) => {
  return (
    <aside className="w-[300px] border-r overflow-y-auto">
      <h2 className="text-lg font-bold p-4">Чаты</h2>
      {loading ? (
        <div className="text-center text-gray-500 p-4">Загрузка...</div>
      ) : chats.length === 0 ? (
        <div className="text-center text-gray-500 p-4">Чатов пока нет</div>
      ) : (
        <ul className="space-y-1 px-2 pb-4">
          {chats.map((chat) => (
            <ChatListItem
              key={chat.id}
              chat={chat}
              selected={chat.id === selectedChatId}
              onSelect={() => onSelectChat(chat.id)}
            />
          ))}
        </ul>
      )}
    </aside>
  );
};

export default ChatList;

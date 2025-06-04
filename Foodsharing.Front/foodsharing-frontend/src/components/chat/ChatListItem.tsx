import { FC } from "react";
import { ChatDTO } from "../../types/chat";
import { formatDateHumanFriendly } from "../../utils/formatDate";
import { FaCheck, FaCheckDouble, FaExclamation } from "react-icons/fa";
import { StaticAPI } from "../../services/api";

interface Props {
  chat: ChatDTO;
  selected: boolean;
  onSelect: () => void;
}

const ChatListItem: FC<Props> = ({ chat, selected, onSelect }) => {
  const { interlocutor, message } = chat;
  const avatarUrl = interlocutor.image
    ? `${StaticAPI.defaults.baseURL}${interlocutor.image}`
    : null;

const getStatusIcon = () => {
    if (!message) return null;
    
    if (!message.isMy && message.status === "Не прочитано") {
        return <div className="w-2 h-2 rounded-full bg-primary" />;
    }
    
    switch (message.status) {
        case "Прочитано":
        return message.isMy && <FaCheckDouble className="text-green-500" />;
        case "Не прочитано":
        return message.isMy && <FaCheck className="text-gray-500" />;
        case "Не доставлено":
        return message.isMy && <FaExclamation className="text-red-500" />;
        default:
        return null;
    }
    };

  const initials = interlocutor.firstName?.[0]?.toUpperCase() || "?";

  return (
    <div
      onClick={onSelect}
      className={`flex gap-3 px-4 py-3 cursor-pointer hover:bg-gray-100 rounded-lg ${
        selected ? "bg-gray-100" : ""
      }`}
    >
        {avatarUrl ? (
        <img
            src={avatarUrl}
            alt={interlocutor.userName}
            className="w-12 h-12 rounded-full object-cover"
        />
        ) : (
        <div className="w-12 h-12 rounded-full bg-gray-300 flex items-center justify-center text-white font-bold text-lg">
            {initials}
        </div>
        )}

      <div className="flex-1">
        <div className="flex justify-between items-center mb-1">
          <span className="font-medium truncate">
            {interlocutor.firstName}
          </span>
          <span className="text-xs text-gray-500">
            {message?.date ? formatDateHumanFriendly(message.date) : ""}
          </span>
        </div>
        <div className="flex justify-between gap-2 text-sm text-gray-600">
          <span className="truncate">{message?.text || "Нет сообщений"}</span>
        </div>
      </div>
    </div>
  );
};

export default ChatListItem;

import { FC } from "react";
import { ChatDTO } from "../../types/chat";
import { formatDateHumanFriendly } from "../../utils/formatDate";
import { FaCheck, FaCheckDouble } from "react-icons/fa";
import { StaticAPI } from "../../services/api";
import { useCurrentUserId } from "../../hooks/useCurrentUserId";

interface Props {
  chat: ChatDTO;
  selected: boolean;
  onSelect: () => void;
}

const ChatListItem: FC<Props> = ({ chat, selected, onSelect }) => {
  const { interlocutor, message, unreadCount } = chat;
  const currentUserId = useCurrentUserId();
  const avatarUrl = interlocutor.image
    ? `${StaticAPI.defaults.baseURL}${interlocutor.image}`
    : null;

  // Определяем, является ли последнее сообщение своим
  const isLastOwn = message?.sender.userId === currentUserId;

  // Выбираем нужную иконку по статусу
  const renderStatusIcon = () => {
    if (!message || !isLastOwn) return null;
    if (message.status === "Доставлено") {
      return <FaCheck className="text-gray-400 mr-1" size={12} />;
    }
    if (message.status === "Прочитано") {
      return <FaCheckDouble className="text-blue-500 mr-1" size={12} />;
    }
    // Если status = "IsNotRead", можно не отображать ничего (или серую галку)
    return null;
  };

  return (
    <div
      className="relative flex items-center p-4 space-x-3 cursor-pointer hover:bg-gray-100"
      onClick={onSelect}
    >
      {selected && (
        <div className="absolute left-0 w-1 h-full bg-blue-500"></div>
      )}
      <img
        className="w-10 h-10 rounded-full object-cover"
        src={avatarUrl || "/default-avatar.png"}
        alt="avatar"
      />
      <div className="flex-1">
        <div className="flex justify-between">
          <span className="font-bold">
            {interlocutor.firstName} {interlocutor.lastName}
          </span>
          <span className="text-xs text-gray-500">
            {message?.date ? formatDateHumanFriendly(message.date) : ""}
          </span>
        </div>

        <div className="flex justify-between items-center text-sm text-gray-600 mt-1">
          <div className="flex items-center">
            {renderStatusIcon()}
            <span className="truncate">{message?.text || "Нет сообщений"}</span>
          </div>

          {unreadCount > 0 && (
            <span className="ml-2 bg-red-500 text-white text-xs font-semibold px-2 py-0.5 rounded-full">
              {unreadCount}
            </span>
          )}
        </div>
      </div>
    </div>
  );
};

export default ChatListItem;

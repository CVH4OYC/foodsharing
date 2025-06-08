import { FC } from "react";
import { ChatDTO } from "../../types/chat";
import { formatDateHumanFriendly } from "../../utils/formatDate";
import { FaCheck, FaCheckDouble } from "react-icons/fa";
import { StaticAPI } from "../../services/api";
import { useCurrentUserId } from "../../hooks/useCurrentUserId";
import AvatarOrInitials from "./AvatarOrInitials";

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

  const fullName = `${interlocutor.firstName}`.trim();
  const isLastOwn = message?.sender.userId === currentUserId;

  const renderStatusIcon = () => {
    if (!message || !isLastOwn) return null;
    if (message.status === "Доставлено") {
      return <FaCheck className="text-gray-400 mr-1" size={12} />;
    }
    if (message.status === "Прочитано") {
      return <FaCheckDouble className="text-blue-500 mr-1" size={12} />;
    }
    return null;
  };

  const truncatedText =
    message?.text && message.text.length > 100
      ? message.text.slice(0, 100) + "..."
      : message?.text || "Нет сообщений";

  return (
    <div
      className="relative flex items-center p-4 space-x-3 cursor-pointer hover:bg-gray-100"
      onClick={onSelect}
    >
      {selected && (
        <div className="absolute left-0 w-1 h-full bg-blue-500"></div>
      )}

      <AvatarOrInitials
        name={interlocutor.firstName}
        imageUrl={avatarUrl}
        size={40}
      />

      <div className="flex-1 min-w-0">
        <div className="flex justify-between">
          <span className="font-bold truncate">{fullName}</span>
          <span className="text-xs text-gray-500 whitespace-nowrap">
            {message?.date ? formatDateHumanFriendly(message.date) : ""}
          </span>
        </div>

        <div className="flex justify-between items-center text-sm text-gray-600 mt-1">
          <div className="flex items-center min-w-0">
            {renderStatusIcon()}
            <span className="truncate">{truncatedText}</span>
          </div>

          {unreadCount > 0 && (
            <span className="ml-2 bg-red-500 text-white text-xs font-semibold px-2 py-0.5 rounded-full shrink-0">
              {unreadCount}
            </span>
          )}
        </div>
      </div>
    </div>
  );
};

export default ChatListItem;

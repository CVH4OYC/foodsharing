import { FC } from "react";
import { FiPaperclip } from "react-icons/fi";
import { StaticAPI } from "../../services/api";
import { MessageDTO } from "../../types/chat";

interface Props {
  message: MessageDTO;
  isOwn: boolean; // true, если это сообщение текущего пользователя
}

const MessageItem: FC<Props> = ({ message, isOwn }) => {
  const bubbleClasses = isOwn
    ? "bg-primary text-white self-end ml-auto"
    : "bg-gray-100 text-gray-800 self-start";

  // Функция для преобразования backend-статуса в «человеческую» строку и иконку
  const renderStatus = () => {
    if (!isOwn || !message.status) return null;

    switch (message.status) {
      case "Не прочитано":
        return (
          <span className="text-[10px] text-gray-300 flex items-center">
            <FiPaperclip /* или своя иконка для «не прочитано» */ />
            <span className="ml-1">Не прочитано</span>
          </span>
        );
      case "Доставлено":
        return (
          <span className="text-[10px] text-gray-300 flex items-center">
            <span className="mr-1">✓</span>
            <span>Доставлено</span>
          </span>
        );
      case "Прочитано":
        return (
          <span className="text-[10px] text-green-300 flex items-center">
            <span className="mr-1">✓✓</span>
            <span>Прочитано</span>
          </span>
        );
      default:
        return null;
    }
  };

  return (
    <div className={`flex flex-col mb-2 ${bubbleClasses} p-2 rounded-lg max-w-[70%]`}>
      {message.text && <p className="break-words">{message.text}</p>}
      {message.image && (
        <img
          src={`${StaticAPI.defaults.baseURL}${message.image}`}
          alt="img"
          className="max-w-full mt-2 rounded"
        />
      )}
      {message.file && (
        <a
          href={`${StaticAPI.defaults.baseURL}${message.file}`}
          target="_blank"
          rel="noopener noreferrer"
          className="flex items-center mt-2 text-blue-500"
        >
          <FiPaperclip className="mr-1" /> Скачать файл
        </a>
      )}

      <div className="flex justify-between items-center mt-1">
        <span className="text-[10px] text-gray-500">
          {new Date(message.date).toLocaleTimeString("ru-RU", {
            hour: "2-digit",
            minute: "2-digit",
          })}
        </span>
        {renderStatus()}
      </div>
    </div>
  );
};

export default MessageItem;

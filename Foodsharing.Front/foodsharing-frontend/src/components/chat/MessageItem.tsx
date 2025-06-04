// components/chat/MessageItem.tsx
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

  return (
    <div className={`max-w-[85%] px-4 py-2 rounded-xl flex flex-col gap-1 ${bubbleClasses}`}>
      {message.text && <div className="text-sm whitespace-pre-line">{message.text}</div>}

      {message.image && (
        <a
          href={`${StaticAPI.defaults.baseURL}${message.image}`}
          target="_blank"
          rel="noopener noreferrer"
        >
          <img
            src={`${StaticAPI.defaults.baseURL}${message.image}`}
            alt="attached"
            className="max-w-[200px] max-h-[200px] rounded-lg border"
          />
        </a>
      )}

      {message.file && (
        <a
          href={`${StaticAPI.defaults.baseURL}${message.file}`}
          download
          className="flex items-center gap-2 text-sm underline hover:text-blue-700"
        >
          <FiPaperclip size={16} />
          {message.file.split("/").pop() || "Скачать файл"}
        </a>
      )}

      <div className="text-xs mt-1 opacity-70 text-right flex justify-end items-center gap-2">
        <span>
          {new Date(message.date).toLocaleTimeString("ru-RU", {
            hour: "2-digit",
            minute: "2-digit",
          })}
        </span>
        {isOwn && (
          <span className={`text-[10px] ${message.status === "Прочитано" ? "text-green-300" : "text-gray-300"}`}>
            ✓ {message.status}
          </span>
        )}
      </div>
    </div>
  );
};

export default MessageItem;

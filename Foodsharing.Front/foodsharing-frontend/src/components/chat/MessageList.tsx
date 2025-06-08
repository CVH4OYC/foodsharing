// components/chat/MessageList.tsx
import { FC, useRef, useEffect, useCallback } from "react";
import { MessageDTO } from "../../types/chat";
import MessageItem from "./MessageItem";
import { useCurrentUserId } from "../../hooks/useCurrentUserId";

interface Props {
  messages: MessageDTO[];
  loadingOlder: boolean;
  onLoadMore: () => void;
}

const MessageList: FC<Props> = ({ messages, loadingOlder, onLoadMore }) => {
  const scrollContainerRef = useRef<HTMLDivElement | null>(null);
  const currentUserId = useCurrentUserId();

  // Скролл вниз при монтировании и при появлении новых сообщений
  const scrollToBottom = () => {
    const div = scrollContainerRef.current;
    if (div) {
      requestAnimationFrame(() => {
        div.scrollTop = div.scrollHeight;
      });
    }
  };

  useEffect(() => {
    scrollToBottom();
  }, [messages]);

  // Обработчик скролла для подгрузки старых сообщений
  const handleScroll = useCallback(() => {
    const div = scrollContainerRef.current;
    if (!div) return;
    // Если прокрутка близка к верху и не идёт текущая загрузка — грузим ещё
    if (div.scrollTop === 0 && !loadingOlder) {
      onLoadMore();
    }
  }, [onLoadMore, loadingOlder]);

  useEffect(() => {
    const div = scrollContainerRef.current;
    if (!div) return;
    div.addEventListener("scroll", handleScroll);
    return () => div.removeEventListener("scroll", handleScroll);
  }, [handleScroll]);

  // Разметка
  return (
    <div
      ref={scrollContainerRef}
      className="flex-1 overflow-y-auto px-4 py-2 flex flex-col gap-2"
    >
      {loadingOlder && (
        <div className="text-center text-gray-500 py-2">Загрузка...</div>
      )}

      {messages.length > 0 ? (
        messages.map((msg, index) => {
          const prevMsg = messages[index - 1];
          const currDate = new Date(msg.date).toDateString();
          const prevDate = prevMsg ? new Date(prevMsg.date).toDateString() : null;
          const showDateSeparator = currDate !== prevDate;

          return (
            <div key={msg.id || index} className="flex flex-col gap-1">
              {showDateSeparator && (
                <div className="text-center text-xs text-gray-500 my-2">
                  {new Date(msg.date).toLocaleDateString("ru-RU", {
                    day: "2-digit",
                    month: "2-digit",
                    year: "numeric",
                  })}
                </div>
              )}
              <MessageItem
                message={msg}
                isOwn={msg.sender.userId === currentUserId}
              />
            </div>
          );
        })
      ) : (
        !loadingOlder && (
          <div className="text-center text-gray-400 mt-4">Сообщений пока нет</div>
        )
      )}
    </div>
  );
};

export default MessageList;

// src/components/chat/ChatWindow.tsx
import { FC, useEffect, useState } from "react";
import { ChatWithMessagesDTO, MessageDTO } from "../../types/chat";
import { API, StaticAPI } from "../../services/api";

interface Props {
  chatId: string;
}

const ChatWindow: FC<Props> = ({ chatId }) => {
  const [chat, setChat] = useState<ChatWithMessagesDTO | null>(null);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    const fetchChat = async () => {
      setLoading(true);
      try {
        const res = await API.get(`/chat/${chatId}`);
        setChat(res.data);
      } catch (err) {
        console.error("Ошибка загрузки чата", err);
      } finally {
        setLoading(false);
      }
    };

    fetchChat();
  }, [chatId]);

  if (loading) {
    return <div className="flex-1 flex items-center justify-center text-gray-500">Загрузка чата...</div>;
  }

  if (!chat) {
    return <div className="flex-1 flex items-center justify-center text-gray-500">Чат не найден</div>;
  }

  const { interlocutor, messages } = chat;
  const avatar = interlocutor.image
    ? `${StaticAPI.defaults.baseURL}${interlocutor.image}`
    : null;
  const initials = interlocutor.firstName?.[0]?.toUpperCase() || "?";

  return (
    <div className="flex-1 flex flex-col h-full">
      {/* Шапка */}
      <div className="border-b px-4 py-3 flex items-center gap-3">
        {avatar ? (
          <img src={avatar} alt={interlocutor.userName} className="w-10 h-10 rounded-full object-cover" />
        ) : (
          <div className="w-10 h-10 rounded-full bg-gray-300 flex items-center justify-center text-white font-bold">
            {initials}
          </div>
        )}
        <span className="font-semibold">{interlocutor.firstName} {interlocutor.lastName}</span>
      </div>

      {/* Сообщения */}
      <div className="flex-1 overflow-y-auto px-4 py-2 space-y-2">
        {messages && messages.length > 0 ? (
          messages.map((msg, index) => (
            <div
              key={index}
              className={`max-w-[70%] px-4 py-2 rounded-xl ${
                msg.isMy ? "bg-primary text-white self-end ml-auto" : "bg-gray-100 text-gray-800 self-start"
              }`}
            >
              <div className="text-sm whitespace-pre-line">{msg.text}</div>
              <div className="text-xs mt-1 opacity-70 text-right">{new Date(msg.date).toLocaleTimeString()}</div>
            </div>
          ))
        ) : (
          <div className="text-center text-gray-400 mt-4">Сообщений пока нет</div>
        )}
      </div>

      {/* Футер (ввод сообщения — пока заготовка) */}
      <div className="border-t px-4 py-3">
        <input
          type="text"
          placeholder="Введите сообщение..."
          className="w-full border rounded-xl px-4 py-2 outline-none"
          disabled
        />
      </div>
    </div>
  );
};

export default ChatWindow;

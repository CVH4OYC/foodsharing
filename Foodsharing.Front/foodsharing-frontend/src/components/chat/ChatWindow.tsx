import { FC, useEffect, useState, useRef } from "react";
import { ChatWithMessagesDTO } from "../../types/chat";
import { API, StaticAPI } from "../../services/api";

interface Props {
  chatId: string | null;
  interlocutorId?: string;
}

const ChatWindow: FC<Props> = ({ chatId, interlocutorId }) => {
  const [chat, setChat] = useState<ChatWithMessagesDTO | null>(null);
  const [loading, setLoading] = useState(true);
  const [tempMessage, setTempMessage] = useState("");
  const [sending, setSending] = useState(false);
  const [creatingChatId, setCreatingChatId] = useState<string | null>(null);
  const [interlocutorName, setInterlocutorName] = useState<string>("");
  const intervalRef = useRef<NodeJS.Timeout | null>(null);

  const actualChatId = chatId || creatingChatId;

  const fetchChat = async (id: string, updateOnlyMessages = false) => {
    try {
      const res = await API.get(`/chat/${id}`);
      if (updateOnlyMessages) {
        setChat((prev) =>
          prev ? { ...prev, messages: res.data.messages } : res.data
        );
      } else {
        setChat(res.data);
      }
    } catch (err) {
      console.error("Ошибка загрузки чата", err);
      if (!updateOnlyMessages) setChat(null);
    } finally {
      if (!updateOnlyMessages) setLoading(false);
    }
  };

  const fetchInterlocutor = async () => {
    if (!interlocutorId) return;
    try {
      const res = await API.get(`/user/${interlocutorId}`);
      const user = res.data;
      setInterlocutorName(`${user.firstName || ""} ${user.lastName || ""}`.trim());
    } catch (err) {
      console.error("Ошибка загрузки данных пользователя", err);
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    if (chatId) {
      fetchChat(chatId);
    } else if (interlocutorId) {
      fetchInterlocutor();
      setLoading(false);
    }

    return () => {
      if (intervalRef.current) clearInterval(intervalRef.current);
    };
  }, [chatId, interlocutorId]);

  useEffect(() => {
    if (!actualChatId) return;
    intervalRef.current = setInterval(() => {
      fetchChat(actualChatId, true);
    }, 5000);
    return () => {
      if (intervalRef.current) clearInterval(intervalRef.current);
    };
  }, [actualChatId]);

  const handleSend = async () => {
    if (!tempMessage.trim() || (!actualChatId && !interlocutorId)) return;
    setSending(true);

    try {
      let finalChatId = actualChatId;

      if (!finalChatId && interlocutorId) {
        const res = await API.post<string>("/chat", null, {
          params: { otherUserId: interlocutorId },
        });
        finalChatId = res.data;
        setCreatingChatId(finalChatId);
        await fetchChat(finalChatId);
      }

      if (!finalChatId) throw new Error("Chat ID is not available");

      await API.post("/message", {
        chatId: finalChatId,
        text: tempMessage,
      });

      setTempMessage("");
      await fetchChat(finalChatId, true);
    } catch (err) {
      console.error("Ошибка при отправке сообщения", err);
    } finally {
      setSending(false);
    }
  };

  if (loading && !chat) {
    return <div className="flex-1 flex items-center justify-center text-gray-500">Загрузка чата...</div>;
  }

  if (!chat && interlocutorId) {
    return (
      <div className="flex-1 flex flex-col h-full">
        <div className="border-b px-4 py-3 flex items-center gap-3">
          <div className="w-10 h-10 rounded-full bg-gray-300 flex items-center justify-center text-white font-bold">
            {interlocutorName[0]?.toUpperCase() || "?"}
          </div>
          <span className="font-semibold">{interlocutorName || "Новый чат"}</span>
        </div>
        <div className="flex-1 flex items-center justify-center text-gray-400">Напишите первое сообщение</div>
        <div className="border-t px-4 py-3">
          <form onSubmit={(e) => { e.preventDefault(); handleSend(); }}>
            <input
              type="text"
              placeholder="Введите сообщение..."
              value={tempMessage}
              onChange={(e) => setTempMessage(e.target.value)}
              className="w-full border rounded-xl px-4 py-2 outline-none"
              disabled={sending}
            />
          </form>
        </div>
      </div>
    );
  }

  if (!chat) {
    return <div className="flex-1 flex items-center justify-center text-gray-500">Чат не найден</div>;
  }

  const { interlocutor, messages } = chat;
  const avatar = interlocutor.image ? `${StaticAPI.defaults.baseURL}${interlocutor.image}` : null;
  const initials = interlocutor.firstName?.[0]?.toUpperCase() || "?";

  return (
    <div className="flex-1 flex flex-col h-full">
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
              <div className="text-xs mt-1 opacity-70 text-right">
                {new Date(msg.date).toLocaleTimeString("ru-RU", {
                  hour: "2-digit",
                  minute: "2-digit",
                })}
              </div>
            </div>
          ))
        ) : (
          <div className="text-center text-gray-400 mt-4">Сообщений пока нет</div>
        )}
      </div>

      <div className="border-t px-4 py-3">
        <form onSubmit={(e) => { e.preventDefault(); handleSend(); }}>
          <input
            type="text"
            placeholder="Введите сообщение..."
            value={tempMessage}
            onChange={(e) => setTempMessage(e.target.value)}
            className="w-full border rounded-xl px-4 py-2 outline-none"
            disabled={sending}
          />
        </form>
      </div>
    </div>
  );
};

export default ChatWindow;

import { FC, useEffect, useState, useRef } from "react";
import { ChatWithMessagesDTO } from "../../types/chat";
import { API, StaticAPI } from "../../services/api";
import { FiSend, FiPaperclip, FiCamera, FiX } from "react-icons/fi";

interface Props {
  chatId: string | null;
  interlocutorId?: string;
  onNewChatCreated?: () => void;
}

const ChatWindow: FC<Props> = ({ chatId, interlocutorId, onNewChatCreated }) => {
  const [chat, setChat] = useState<ChatWithMessagesDTO | null>(null);
  const [loading, setLoading] = useState(true);
  const [tempMessage, setTempMessage] = useState("");
  const [sending, setSending] = useState(false);
  const [creatingChatId, setCreatingChatId] = useState<string | null>(null);
  const [interlocutorName, setInterlocutorName] = useState<string>("");
  const [file, setFile] = useState<File | null>(null);
  const [image, setImage] = useState<File | null>(null);
  const actualChatId = chatId || creatingChatId;
  const scrollContainerRef = useRef<HTMLDivElement | null>(null);

  const fetchChat = async (id: string) => {
    try {
      const res = await API.get(`/chat/${id}`);
      setChat(res.data);
    } catch (err) {
      console.error("Ошибка загрузки чата", err);
      setChat(null);
    }
  };

  const fetchInterlocutor = async () => {
    if (!interlocutorId) return;
    try {
      const res = await API.get(`/user/${interlocutorId}`);
      const user = res.data;
      setInterlocutorName(`${user.firstName || ""} ${user.lastName || ""}`.trim());
    } catch (err) {
      console.error("Ошибка загрузки пользователя", err);
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    if (chatId) {
      fetchChat(chatId);
      setLoading(false);
    } else if (interlocutorId) {
      fetchInterlocutor();
    }
  }, [chatId, interlocutorId]);

  useEffect(() => {
    if (!actualChatId) return;
    const interval = setInterval(() => fetchChat(actualChatId), 5000);
    return () => clearInterval(interval);
  }, [actualChatId]);

  useEffect(() => {
    scrollContainerRef.current?.scrollTo({
      top: scrollContainerRef.current.scrollHeight,
      behavior: "smooth",
    });
  }, [chat?.messages]);

  const handleSend = async () => {
    if (!tempMessage.trim() && !image && !file) return;
    if (!actualChatId && !interlocutorId) return;

    setSending(true);
    try {
      let finalChatId = actualChatId;
      if (!finalChatId && interlocutorId) {
        const res = await API.post("/chat", null, {
          params: { otherUserId: interlocutorId },
        });
        finalChatId = res.data;
        setCreatingChatId(finalChatId);
        onNewChatCreated?.();
        if (finalChatId)
        {
          await fetchChat(finalChatId);
        }
      }

      if (!finalChatId) throw new Error("Chat ID is not available");

      const formData = new FormData();
      formData.append("ChatId", finalChatId);
      formData.append("Text", tempMessage);
      if (image) formData.append("Image", image);
      if (file) formData.append("File", file);

      await API.post("/message", formData, {
        headers: { "Content-Type": "multipart/form-data" },
      });

      setTempMessage("");
      setImage(null);
      setFile(null);
      await fetchChat(finalChatId);

      scrollContainerRef.current?.scrollTo({
        top: scrollContainerRef.current.scrollHeight,
        behavior: "smooth",
      });
    } catch (err) {
      console.error("Ошибка при отправке сообщения", err);
    } finally {
      setSending(false);
    }
  };

  const renderFilePreviews = () => (
    (image || file) && (
      <div className="flex gap-4 mb-2 items-center">
        {image && (
          <div className="relative w-20 h-20">
            <img src={URL.createObjectURL(image)} alt="preview" className="w-20 h-20 object-cover rounded-lg border" />
            <button onClick={() => setImage(null)} type="button" className="absolute -top-2 -right-2 bg-white rounded-full shadow p-1 hover:bg-red-100">
              <FiX size={16} />
            </button>
          </div>
        )}
        {file && (
          <div className="relative flex items-center gap-2 px-3 py-2 bg-gray-100 rounded-lg border max-w-xs">
            <FiPaperclip size={20} className="text-gray-500" />
            <div className="text-sm truncate">{file.name}</div>
            <button onClick={() => setFile(null)} type="button" className="absolute -top-2 -right-2 bg-white rounded-full shadow p-1 hover:bg-red-100">
              <FiX size={16} />
            </button>
          </div>
        )}
      </div>
    )
  );

  const renderForm = () => (
    <>
      {renderFilePreviews()}
      <form onSubmit={(e) => { e.preventDefault(); handleSend(); }} className="flex gap-2 items-center">
        <label className="cursor-pointer text-primary">
          <FiCamera size={20} />
          <input type="file" onChange={(e) => setImage(e.target.files?.[0] || null)} className="hidden" />
        </label>
        <label className="cursor-pointer text-primary">
          <FiPaperclip size={20} />
          <input type="file" onChange={(e) => setFile(e.target.files?.[0] || null)} className="hidden" />
        </label>
        <input
          type="text"
          placeholder="Введите сообщение..."
          value={tempMessage}
          onChange={(e) => setTempMessage(e.target.value)}
          className="flex-1 border rounded-xl px-4 py-2 outline-none"
          disabled={sending}
        />
        <button type="submit" className="text-primary hover:text-green-600" disabled={sending}>
          <FiSend size={24} />
        </button>
      </form>
    </>
  );

  const renderAvatarOrInitials = (name?: string, url?: string) =>
    url ? (
      <img src={url} alt="avatar" className="w-10 h-10 rounded-full object-cover" />
    ) : (
      <div className="w-10 h-10 rounded-full bg-gray-300 flex items-center justify-center text-white font-bold">
        {name?.[0]?.toUpperCase() || "?"}
      </div>
    );

  if (loading && !chat) {
    return <div className="flex-1 flex items-center justify-center text-gray-500">Загрузка чата...</div>;
  }

  // 👉 Отображение экрана, если чат ещё не создан
  if (!chat && interlocutorId) {
    return (
      <div className="flex-1 flex flex-col h-full">
        <div className="border-b px-4 py-3 flex items-center gap-3">
          {renderAvatarOrInitials(interlocutorName)}
          <span className="font-semibold">{interlocutorName || "Новый чат"}</span>
        </div>
        <div className="flex-1 flex items-center justify-center text-gray-400">
          Напишите первое сообщение
        </div>
        <div className="border-t px-4 py-3">{renderForm()}</div>
      </div>
    );
  }

  if (!chat) {
    return <div className="flex-1 flex items-center justify-center text-gray-500">Чат не найден</div>;
  }

  const { interlocutor, messages } = chat;
  const avatar = interlocutor.image ? `${StaticAPI.defaults.baseURL}${interlocutor.image}` : undefined;

  return (
    <div className="flex-1 flex flex-col h-full">
      <div className="border-b px-4 py-3 flex items-center gap-3">
        {renderAvatarOrInitials(interlocutor.firstName, avatar)}
        <span className="font-semibold">{interlocutor.firstName} {interlocutor.lastName}</span>
      </div>
      <div ref={scrollContainerRef} className="flex-1 overflow-y-auto px-4 py-2 space-y-2">
        {messages && messages.length > 0 ? (
          messages.map((msg, index) => (
            <div key={index} className={`max-w-[70%] px-4 py-2 rounded-xl flex flex-col gap-1 ${msg.isMy ? "bg-primary text-white self-end ml-auto" : "bg-gray-100 text-gray-800 self-start"}`}>
              {msg.text && <div className="text-sm whitespace-pre-line">{msg.text}</div>}
              {msg.image && (
                <a href={`${StaticAPI.defaults.baseURL}${msg.image}`} target="_blank" rel="noopener noreferrer">
                  <img src={`${StaticAPI.defaults.baseURL}${msg.image}`} alt="attached" className="max-w-[200px] max-h-[200px] rounded-lg border" />
                </a>
              )}
              {msg.file && (
                <a href={`${StaticAPI.defaults.baseURL}${msg.file}`} download className="flex items-center gap-2 text-sm underline hover:text-blue-700">
                  <FiPaperclip size={16} />
                  {msg.file.split("/").pop() || "Скачать файл"}
                </a>
              )}
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
      <div className="border-t px-4 py-3">{renderForm()}</div>
    </div>
  );
};

export default ChatWindow;

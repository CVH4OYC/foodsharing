import { FC, useEffect, useState, useRef, useCallback } from "react";
import { ChatWithMessagesDTO, MessageDTO, UserDTO } from "../../types/chat";
import { API, StaticAPI } from "../../services/api";
import { FiSend, FiPaperclip, FiCamera, FiX } from "react-icons/fi";
import { useChatSignalR } from "../../hooks/useChatSignalR";
import { useCurrentUserId } from "../../hooks/useCurrentUserId";


interface Props {
  chatId: string | null;
  interlocutorId?: string;
  onNewChatCreated?: () => void;
}

const PAGE_SIZE = 20;

const ChatWindow: FC<Props> = ({ chatId, interlocutorId, onNewChatCreated }) => {
  const [messages, setMessages] = useState<MessageDTO[]>([]);
  const [interlocutor, setInterlocutor] = useState<{
    firstName: string;
    lastName: string;
    image: string | null;
    userId: string;
  } | null>(null);
  const [page, setPage] = useState(1);
  const [hasMore, setHasMore] = useState(true);
  const [loading, setLoading] = useState(true);
  const [loadingMessages, setLoadingMessages] = useState(false);
  const [tempMessage, setTempMessage] = useState("");
  const [sending, setSending] = useState(false);
  const [creatingChatId, setCreatingChatId] = useState<string | null>(null);
  const [file, setFile] = useState<File | null>(null);
  const [image, setImage] = useState<File | null>(null);
  const inputRef = useRef<HTMLInputElement | null>(null);

  
  const scrollContainerRef = useRef<HTMLDivElement | null>(null);
  const currentUserId = useCurrentUserId();
  const actualChatId = chatId || creatingChatId;

  // Сброс состояния при смене чата
  useEffect(() => {
    setMessages([]);
    setPage(1);
    setHasMore(true);
    setInterlocutor(null);
    setCreatingChatId(null);
  }, [chatId, interlocutorId]);

  // Загрузка данных чата
  useEffect(() => {
    const fetchData = async () => {
      setLoading(true);
      try {
        if (actualChatId) {
          // Загрузка метаданных чата
          const metaRes = await API.get<ChatWithMessagesDTO>(`/chat/${actualChatId}`, {
            params: { onlyMeta: true }
          });
          
          const interlocutorData = metaRes.data.interlocutor;
          setInterlocutor({
            firstName: interlocutorData.firstName || "",
            lastName: interlocutorData.lastName || "",
            image: interlocutorData.image || null,
            userId: interlocutorData.userId
          });
          
          // Загрузка сообщений
          await fetchMessages(actualChatId, 1);
        } else if (interlocutorId) {
          // Загрузка собеседника
          const userRes = await API.get<UserDTO>(`/user/${interlocutorId}`);
          setInterlocutor({
            firstName: userRes.data.firstName || "",
            lastName: userRes.data.lastName || "",
            image: userRes.data.image || null,
            userId: interlocutorId
          });
        }
      } catch (err) {
        console.error("Ошибка загрузки данных", err);
      } finally {
        setLoading(false);
      }
    };

    fetchData();
  }, [actualChatId, interlocutorId]);


  useEffect(() => {
    if (!loading) {
      inputRef.current?.focus();
    }
  }, [loading]);

  
  // Пагинация сообщений
  const fetchMessages = async (id: string, pageToLoad: number) => {
    setLoadingMessages(true);
    try {
      const res = await API.get<ChatWithMessagesDTO>(`/chat/${id}`, {
        params: { page: pageToLoad, pageSize: PAGE_SIZE }
      });
      
      const newMessages = res.data.messages || [];

      if (pageToLoad === 1) {
        setMessages(newMessages);
        // Прокрутка вниз после загрузки первой страницы
        setTimeout(() => scrollToBottom(), 0);
      } else {
        // Добавляем старые сообщения в начало
        setMessages(prev => [...newMessages, ...prev]);
      }
      
      setHasMore(newMessages.length === PAGE_SIZE);
    } catch (err) {
      console.error("Ошибка загрузки сообщений", err);
    } finally {
      setLoadingMessages(false);
    }
  };

  // Подгрузка старых сообщений
  const loadMore = async () => {
    if (!actualChatId || !hasMore || loadingMessages) return;
    const nextPage = page + 1;
    await fetchMessages(actualChatId, nextPage);
    setPage(nextPage);
  };

  // Обработчик скролла
  const handleScroll = useCallback(() => {
    const div = scrollContainerRef.current;
    if (!div || div.scrollTop > 100 || loadingMessages) return;
    
    // Проверяем, достигли ли верха контейнера
    if (div.scrollTop === 0) {
      loadMore();
    }
  }, [actualChatId, page, hasMore, loadingMessages]);

  // Подписка на события скролла
  useEffect(() => {
    const div = scrollContainerRef.current;
    if (!div) return;
    
    div.addEventListener("scroll", handleScroll);
    return () => div.removeEventListener("scroll", handleScroll);
  }, [handleScroll]);

  // SignalR обработчики
  useChatSignalR({
    conversationId: actualChatId || "",
    currentUserId: currentUserId || "",
    onNewMessage: (message: MessageDTO) => {
      // Добавляем новые сообщения в конец
      setMessages(prev => [...prev, message]);
      scrollToBottom();
    },
    onMessagesRead: () => {
      if (!currentUserId) return;
      setMessages(prev =>
        prev.map(m =>
          m.status === "Не прочитано" && m.sender.userId === currentUserId
            ? { ...m, status: "Прочитано" }
            : m
        )
      );
    },
  });

  useEffect(() => {
    scrollToBottom();
  }, [messages]);

  const scrollToBottom = () => {
    const div = scrollContainerRef.current;
    if (div) {
      requestAnimationFrame(() => {
        div.scrollTop = div.scrollHeight;
      });
    }
  };
  


  // Отправка сообщения
  const handleSend = async () => {
    if (!tempMessage.trim() && !image && !file) return;
    if (!actualChatId && !interlocutorId) return;

    setSending(true);
    try {
      let finalChatId = actualChatId;
      
      // Создание нового чата
      if (!finalChatId && interlocutorId) {
        const res = await API.post<string>("/chat", null, { 
          params: { otherUserId: interlocutorId } 
        });
        finalChatId = res.data;
        setCreatingChatId(finalChatId);
        onNewChatCreated?.();
      }

      if (!finalChatId) throw new Error("Chat ID is not available");

      // Отправка сообщения
      const formData = new FormData();
      formData.append("ChatId", finalChatId);
      formData.append("Text", tempMessage);
      if (image) formData.append("Image", image);
      if (file) formData.append("File", file);

      await API.post("/message", formData, {
        headers: { "Content-Type": "multipart/form-data" },
      });

      // Сброс формы
      setTempMessage("");
      setImage(null);
      setFile(null);
      requestAnimationFrame(() => inputRef.current?.focus());
    } catch (err) {
      console.error("Ошибка при отправке сообщения", err);
    } finally {
      setSending(false);
    }
  };

  // Превью прикрепленных файлов
  const renderFilePreviews = () => (
    (image || file) && (
      <div className="flex gap-4 mb-2 items-center">
        {image && (
          <div className="relative w-20 h-20">
            <img 
              src={URL.createObjectURL(image)} 
              alt="preview" 
              className="w-20 h-20 object-cover rounded-lg border" 
            />
            <button 
              onClick={() => setImage(null)} 
              className="absolute -top-2 -right-2 bg-white rounded-full shadow p-1 hover:bg-red-100"
            >
              <FiX size={16} />
            </button>
          </div>
        )}
        {file && (
          <div className="relative flex items-center gap-2 px-3 py-2 bg-gray-100 rounded-lg border max-w-xs">
            <FiPaperclip size={20} className="text-gray-500" />
            <div className="text-sm truncate">{file.name}</div>
            <button 
              onClick={() => setFile(null)} 
              className="absolute -top-2 -right-2 bg-white rounded-full shadow p-1 hover:bg-red-100"
            >
              <FiX size={16} />
            </button>
          </div>
        )}
      </div>
    )
  );

  // Форма ввода
  const renderForm = () => (
    <>
      {renderFilePreviews()}
      <form 
        onSubmit={(e) => { e.preventDefault(); handleSend(); }} 
        className="flex gap-2 items-center"
      >
        <label className="cursor-pointer text-primary">
          <FiCamera size={20} />
          <input 
            type="file" 
            onChange={(e) => setImage(e.target.files?.[0] || null)} 
            className="hidden" 
            accept="image/*"
          />
        </label>
        <label className="cursor-pointer text-primary">
          <FiPaperclip size={20} />
          <input 
            type="file" 
            onChange={(e) => setFile(e.target.files?.[0] || null)} 
            className="hidden" 
          />
        </label>
        <input
          ref={inputRef}
          type="text"
          placeholder="Введите сообщение..."
          value={tempMessage}
          onChange={(e) => setTempMessage(e.target.value)}
          className="flex-1 border rounded-xl px-4 py-2 outline-none"
          disabled={sending}
        />

        <button 
          type="submit" 
          className="text-primary hover:text-green-600" 
          disabled={sending}
        >
          <FiSend size={24} />
        </button>
      </form>
    </>
  );

  // Рендер аватара
  const renderAvatarOrInitials = (name?: string, url?: string | null) => {
    if (url) {
      return <img src={url} alt="avatar" className="w-10 h-10 rounded-full object-cover" />;
    }
    return (
      <div className="w-10 h-10 rounded-full bg-gray-300 flex items-center justify-center text-white font-bold">
        {name?.[0]?.toUpperCase() || "?"}
      </div>
    );
  };

  // Состояния загрузки
  if (loading) {
    return <div className="flex-1 flex items-center justify-center text-gray-500">Загрузка чата...</div>;
  }

  if (!currentUserId) {
    return <div className="flex-1 flex items-center justify-center text-red-500">Пользователь не авторизован</div>;
  }

  // Рендер нового чата
  if (!actualChatId && interlocutorId && interlocutor) {
    return (
      <div className="flex-1 flex flex-col h-full">
        <div className="border-b px-4 py-3 flex items-center gap-3">
          {renderAvatarOrInitials(interlocutor.firstName, interlocutor.image)}
          <span className="font-semibold">{interlocutor.firstName} {interlocutor.lastName}</span>
        </div>
        <div className="flex-1 flex items-center justify-center text-gray-400">
          Напишите первое сообщение
        </div>
        <div className="border-t px-4 py-3">{renderForm()}</div>
      </div>
    );
  }

  // Чат не найден
  if (!interlocutor) {
    return <div className="flex-1 flex items-center justify-center text-gray-500">Чат не найден</div>;
  }

  // Основной интерфейс чата
  return (
    <div className="flex-1 flex flex-col h-full">
      <div className="border-b px-4 py-3 flex items-center gap-3">
        {renderAvatarOrInitials(interlocutor.firstName, interlocutor.image)}
        <span className="font-semibold">
          {interlocutor.firstName} {interlocutor.lastName}
        </span>
      </div>
      
      <div 
          ref={scrollContainerRef} 
          className="flex-1 overflow-y-auto px-4 py-2 flex flex-col gap-2"
        >
          {loadingMessages && page > 1 && (
            <div className="text-center text-gray-500 py-2">Загрузка...</div>
          )}

          {messages.length > 0 ? (
            messages.map((msg, index) => {
              const prevMsg = messages[index - 1];
              const currDate = new Date(msg.date).toDateString();
              const prevDate = prevMsg ? new Date(prevMsg.date).toDateString() : null;
              const showDate = currDate !== prevDate;

              return (
                <div key={msg.id || index}>
                  {showDate && (
                    <div className="text-center text-xs text-gray-500 my-2">
                      {new Date(msg.date).toLocaleDateString("ru-RU", {
                        day: "2-digit",
                        month: "2-digit",
                        year: "numeric",
                      })}
                    </div>
                  )}
                  <div className={`max-w-[85%] px-4 py-2 rounded-xl flex flex-col gap-1 ${
                    msg.sender.userId === currentUserId 
                      ? "bg-primary text-white self-end ml-auto" 
                      : "bg-gray-100 text-gray-800 self-start"
                  }`}>
                    {msg.text && <div className="text-sm whitespace-pre-line">{msg.text}</div>}
                    {msg.image && (
                      <a 
                        href={`${StaticAPI.defaults.baseURL}${msg.image}`} 
                        target="_blank" 
                        rel="noopener noreferrer"
                      >
                        <img 
                          src={`${StaticAPI.defaults.baseURL}${msg.image}`} 
                          alt="attached" 
                          className="max-w-[200px] max-h-[200px] rounded-lg border" 
                        />
                      </a>
                    )}
                    {msg.file && (
                      <a 
                        href={`${StaticAPI.defaults.baseURL}${msg.file}`} 
                        download 
                        className="flex items-center gap-2 text-sm underline hover:text-blue-700"
                      >
                        <FiPaperclip size={16} />
                        {msg.file.split("/").pop() || "Скачать файл"}
                      </a>
                    )}
                    <div className="text-xs mt-1 opacity-70 text-right flex justify-end items-center gap-2">
                      {new Date(msg.date).toLocaleTimeString("ru-RU", {
                        hour: "2-digit",
                        minute: "2-digit",
                      })}
                      {msg.sender.userId === currentUserId && (
                        <span className={`text-[10px] ${msg.status === "Прочитано" ? "text-green-300" : "text-gray-300"}`}>
                          ✓ {msg.status}
                        </span>
                      )}
                    </div>
                  </div>
                </div>
              );
            })
          ) : (
            !loadingMessages && <div className="text-center text-gray-400 mt-4">Сообщений пока нет</div>
          )}
        </div>

      
      <div className="border-t px-4 py-3">{renderForm()}</div>
    </div>
  );
};

export default ChatWindow;
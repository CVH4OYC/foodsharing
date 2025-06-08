import { FC, useEffect, useState, useCallback } from "react";
import { ChatWithMessagesDTO, MessageDTO, UserDTO } from "../../types/chat";
import { API, StaticAPI } from "../../services/api";
import connection, { startConnection } from "../../services/signalr-chat";
import { useCurrentUserId } from "../../hooks/useCurrentUserId";

import ChatHeader from "../../components/chat/ChatHeader";
import MessageList from "../../components/chat/MessageList";
import MessageInput from "../../components/chat/MessageInput";

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

  const currentUserId = useCurrentUserId();
  const actualChatId = chatId || creatingChatId;

  // Сбрасываем при смене чата
  useEffect(() => {
    setMessages([]);
    setPage(1);
    setHasMore(true);
    setInterlocutor(null);
    setCreatingChatId(null);
  }, [chatId, interlocutorId]);

  // Загрузка метаданных и первой страницы
  useEffect(() => {
    const fetchData = async () => {
      setLoading(true);
      try {
        if (actualChatId) {
          const metaRes = await API.get<ChatWithMessagesDTO>(`/chat/${actualChatId}`, {
            params: { onlyMeta: true },
          });
          const inter = metaRes.data.interlocutor;
          setInterlocutor({
            firstName: inter.firstName || "",
            lastName: inter.lastName || "",
            image: inter.image ? `${StaticAPI.defaults.baseURL}${inter.image}` : null,
            userId: inter.userId,
          });
          await fetchMessages(actualChatId, 1);
        } else if (interlocutorId) {
          const userRes = await API.get<UserDTO>(`/user/${interlocutorId}`);
          setInterlocutor({
            firstName: userRes.data.firstName || "",
            lastName: userRes.data.lastName || "",
            image: userRes.data.image
              ? `${StaticAPI.defaults.baseURL}${userRes.data.image}`
              : null,
            userId: interlocutorId,
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

  // Функция загрузки сообщений
  const fetchMessages = async (id: string, pageToLoad: number) => {
    setLoadingMessages(true);
    try {
      const res = await API.get<ChatWithMessagesDTO>(`/chat/${id}`, {
        params: { page: pageToLoad, pageSize: PAGE_SIZE },
      });
      const newMsgs = res.data.messages || [];
      if (pageToLoad === 1) setMessages(newMsgs);
      else setMessages((prev) => [...newMsgs, ...prev]);
      setHasMore(newMsgs.length === PAGE_SIZE);
    } catch (err) {
      console.error("Ошибка загрузки сообщений", err);
    } finally {
      setLoadingMessages(false);
    }
  };

  // Подгрузка старых
  const loadMore = useCallback(async () => {
    if (!actualChatId || !hasMore || loadingMessages) return;
    const next = page + 1;
    await fetchMessages(actualChatId, next);
    setPage(next);
  }, [actualChatId, hasMore, loadingMessages, page]);

  // ───────── SignalR ──────────────
  useEffect(() => {
    startConnection().catch((e) => console.warn("Ошибка SignalR (ChatWindow)", e));
  }, []);

  // При смене чата — join/leave
  useEffect(() => {
    if (actualChatId) {
      connection.invoke("JoinChat", actualChatId).catch(console.error);
      return () => {
        connection.invoke("LeaveChat", actualChatId).catch(console.error);
      };
    }
  }, [actualChatId]);

  useEffect(() => {
    connection.on("ReceiveMessage", (message: MessageDTO) => {
      setMessages((prev) => [...prev, message]);
    });
    // остальные обработчики, если нужны…
    return () => {
      connection.off("ReceiveMessage");
    };
  }, []);

  // Отправка
  const handleSend = async () => {
    if (!tempMessage.trim() && !image && !file) return;
    if (!actualChatId && !interlocutorId) return;

    setSending(true);
    try {
      let finalChatId = actualChatId;
      // создаём новый чат, если нужно
      if (!finalChatId && interlocutorId) {
        const res = await API.post<string>("/chat", null, {
          params: { otherUserId: interlocutorId },
        });
        finalChatId = res.data;
        setCreatingChatId(finalChatId);
        onNewChatCreated?.();
        // подписываемся на группу нового чата
        await connection.invoke("JoinChat", finalChatId);
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
    } catch (err) {
      console.error("Ошибка при отправке сообщения", err);
    } finally {
      setSending(false);
    }
  };

  // Рендер…
  if (loading) {
    return <div className="flex-1 flex items-center justify-center text-gray-500">Загрузка чата...</div>;
  }
  if (!currentUserId) {
    return <div className="flex-1 flex items-center justify-center text-red-500">Пользователь не авторизован</div>;
  }
  if (!actualChatId && interlocutorId && interlocutor) {
    return (
      <div className="flex-1 flex flex-col h-full">
        <ChatHeader interlocutor={interlocutor} />
        <div className="flex-1 flex items-center justify-center text-gray-400">Напишите первое сообщение</div>
        <div className="border-t px-4 py-3">
          <MessageInput
            tempMessage={tempMessage}
            setTempMessage={setTempMessage}
            image={image}
            setImage={setImage}
            file={file}
            setFile={setFile}
            onSend={handleSend}
            sending={sending}
          />
        </div>
      </div>
    );
  }
  if (!interlocutor) {
    return <div className="flex-1 flex items-center justify-center text-gray-500">Чат не найден</div>;
  }

  return (
    <div className="flex-1 flex flex-col h-full">
      <ChatHeader interlocutor={interlocutor} />
      <MessageList messages={messages} loadingOlder={loadingMessages && page > 1} onLoadMore={loadMore} />
      <div className="border-t px-4 py-3">
        <MessageInput
          tempMessage={tempMessage}
          setTempMessage={setTempMessage}
          image={image}
          setImage={setImage}
          file={file}
          setFile={setFile}
          onSend={handleSend}
          sending={sending}
        />
      </div>
    </div>
  );
};

export default ChatWindow;
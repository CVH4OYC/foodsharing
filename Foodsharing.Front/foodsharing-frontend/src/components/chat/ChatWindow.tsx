// components/chat/ChatWindow.tsx
import { FC, useEffect, useState, useCallback } from "react";
import { ChatWithMessagesDTO, MessageDTO, UserDTO } from "../../types/chat";
import { API, StaticAPI } from "../../services/api";
import { useChatSignalR } from "../../hooks/useChatSignalR";
import { useCurrentUserId } from "../../hooks/useCurrentUserId";

import ChatHeader from "./ChatHeader";
import MessageList from "./MessageList";
import MessageInput from "./MessageInput";

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

  // Сброс состояния при смене чата
  useEffect(() => {
    setMessages([]);
    setPage(1);
    setHasMore(true);
    setInterlocutor(null);
    setCreatingChatId(null);
  }, [chatId, interlocutorId]);

  // Загрузка метаданных и первой страницы сообщений
  useEffect(() => {
    const fetchData = async () => {
      setLoading(true);
      try {
        if (actualChatId) {
          // Получаем только метаданные (имя, аватар)
          const metaRes = await API.get<ChatWithMessagesDTO>(`/chat/${actualChatId}`, {
            params: { onlyMeta: true },
          });

          const interlocutorData = metaRes.data.interlocutor;
          const avatarUrl = interlocutorData.image
            ? `${StaticAPI.defaults.baseURL}${interlocutorData.image}`
            : null;

          setInterlocutor({
            firstName: interlocutorData.firstName || "",
            lastName: interlocutorData.lastName || "",
            image: avatarUrl,
            userId: interlocutorData.userId,
          });

          await fetchMessages(actualChatId, 1);
        } else if (interlocutorId) {
          // Если чата ещё нет, но есть ID собеседника, показываем лишь его инфо
          const userRes = await API.get<UserDTO>(`/user/${interlocutorId}`);
          const avatarUrl = userRes.data.image
            ? `${StaticAPI.defaults.baseURL}${userRes.data.image}`
            : null;

          setInterlocutor({
            firstName: userRes.data.firstName || "",
            lastName: userRes.data.lastName || "",
            image: avatarUrl,
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

  // Загрузка страницы сообщений
  const fetchMessages = async (id: string, pageToLoad: number) => {
    setLoadingMessages(true);
    try {
      const res = await API.get<ChatWithMessagesDTO>(`/chat/${id}`, {
        params: { page: pageToLoad, pageSize: PAGE_SIZE },
      });

      const newMessages = res.data.messages || [];
      if (pageToLoad === 1) {
        setMessages(newMessages);
      } else {
        setMessages((prev) => [...newMessages, ...prev]);
      }
      setHasMore(newMessages.length === PAGE_SIZE);
    } catch (err) {
      console.error("Ошибка загрузки сообщений", err);
    } finally {
      setLoadingMessages(false);
    }
  };

  // Колбек для подгрузки следующей (старой) страницы
  const loadMore = useCallback(async () => {
    if (!actualChatId || !hasMore || loadingMessages) return;
    const nextPage = page + 1;
    await fetchMessages(actualChatId, nextPage);
    setPage(nextPage);
  }, [actualChatId, hasMore, loadingMessages, page]);

  // SignalR: получение новых сообщений, обновление статусов и прочее
  useChatSignalR({
    conversationId: actualChatId || "",
    currentUserId: currentUserId || "",
    onNewMessage: (message: MessageDTO) => {
      setMessages((prev) => [...prev, message]);
    },
    onMessagesRead: () => {
      if (!currentUserId) return;
      setMessages((prev) =>
        prev.map((m) =>
          // Если статус "Не прочитано" и сообщение не от текущего пользователя
          m.status === "Не прочитано" && m.sender.userId !== currentUserId
            ? { ...m, status: "Прочитано" }
            : m
        )
      );
    },
    onMessageStatusUpdate: ({ chatId: chId, messageId, newStatus }) => {
      if (chId !== actualChatId) return;
      setMessages((prev) =>
        prev.map((m) =>
          m.id === messageId
            ? {
                ...m,
                // Преобразуем статус из backend-строки в русский
                status:
                  newStatus === "IsDelivered"
                    ? "Доставлено"
                    : newStatus === "IsRead"
                    ? "Прочитано"
                    : m.status,
              }
            : m
        )
      );
    },
    onChatListUpdate: () => {
      // обновление списка чатов обрабатывается в ChatsPage, здесь ничего не делаем
    },
  });

  // Сразу скроллим вниз при появлении новых сообщений
  useEffect(() => {
    // MessageList сам скроллит вниз, если нужны дополнительные действия —
    // можно здесь добавить реф и вызвать scrollToBottom
  }, [messages]);

  // Отправка сообщения (текст + опционально файл/картинка)
  const handleSend = async () => {
    if (!tempMessage.trim() && !image && !file) return;
    if (!actualChatId && !interlocutorId) return;

    setSending(true);
    try {
      let finalChatId = actualChatId;
      // Если чата ещё нет — создаём новый и уведомляем родителя
      if (!finalChatId && interlocutorId) {
        const res = await API.post<string>("/chat", null, {
          params: { otherUserId: interlocutorId },
        });
        finalChatId = res.data;
        setCreatingChatId(finalChatId);
        onNewChatCreated?.();
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

  // Пока идёт загрузка самого чата (метаданных или первой страницы)
  if (loading) {
    return (
      <div className="flex-1 flex items-center justify-center text-gray-500">
        Загрузка чата...
      </div>
    );
  }

  // Если пользователь не авторизован
  if (!currentUserId) {
    return (
      <div className="flex-1 flex items-center justify-center text-red-500">
        Пользователь не авторизован
      </div>
    );
  }

  // Если у нас ещё нет созданного чата, но есть interlocutorId (новый чат)
  if (!actualChatId && interlocutorId && interlocutor) {
    return (
      <div className="flex-1 flex flex-col h-full">
        <ChatHeader interlocutor={interlocutor} />
        <div className="flex-1 flex items-center justify-center text-gray-400">
          Напишите первое сообщение
        </div>
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

  // Если у нас нет данных interlocutor (что-то пошло не так)
  if (!interlocutor) {
    return (
      <div className="flex-1 flex items-center justify-center text-gray-500">
        Чат не найден
      </div>
    );
  }

  // Основной интерфейс уже существующего чата
  return (
    <div className="flex-1 flex flex-col h-full">
      <ChatHeader interlocutor={interlocutor} />

      <MessageList
        messages={messages}
        loadingOlder={loadingMessages && page > 1}
        onLoadMore={loadMore}
      />

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


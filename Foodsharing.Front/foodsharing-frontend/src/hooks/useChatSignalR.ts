import { useEffect } from "react";
import connection, { startConnection } from "../services/signalr-chat";

type UseChatSignalRProps = {
  conversationId: string;
  currentUserId: string;
  onNewMessage: (message: any) => void;
  onMessagesRead?: () => void;
  onMessageStatusUpdate?: (payload: {
    chatId: string;
    messageId: string;
    newStatus: "Доставлено" | "Прочитано";
  }) => void;
  onChatListUpdate?: (updatedChat: any) => void;

  // ✅ Новый коллбек для уведомления о новом сообщении в неактивном чате
  onChatReceivedMessage?: (chatId: string) => void;
} | null;

export const useChatSignalR = (params: UseChatSignalRProps) => {
  useEffect(() => {
    if (!params) return;

    const {
      conversationId,
      currentUserId,
      onNewMessage,
      onMessagesRead,
      onMessageStatusUpdate,
      onChatListUpdate,
      onChatReceivedMessage,
    } = params;

    let isMounted = true;
    let markAsReadTimeout: any = null;

    startConnection()
      .then(() => {
        if (!isMounted) return;

        // Присоединение к чату
        connection.invoke("JoinChat", conversationId).catch(() =>
          console.warn("❌ Не удалось присоединиться к чату")
        );

        // Получение нового сообщения
        connection.on("ReceiveMessage", (message: any) => {
          // Всегда вызываем основной хендлер
          if (message.chatId === conversationId) {
            onNewMessage(message);
          } else {
            // Если это чат неактивный — вызываем коллбек
            onChatReceivedMessage?.(message.chatId);
          }
        });

        // Обновление статуса одного сообщения
        connection.on("MessageStatusUpdate", (payload: any) => {
          onMessageStatusUpdate?.({
            chatId: payload.chatId,
            messageId: payload.messageId,
            newStatus: payload.newStatus,
          });
        });

        // Отметка всех сообщений как прочитанных
        connection.on("MessagesMarkedAsRead", () => {
          onMessagesRead?.();
        });

        // Обновление карточки чата
        connection.on("ChatListUpdate", (updatedChat: any) => {
          onChatListUpdate?.(updatedChat);
        });

        // Автоматическая отметка как прочитано через 500 мс
        markAsReadTimeout = setTimeout(() => {
          connection
            .invoke("MarkChatAsRead", conversationId)
            .catch(() =>
              console.warn("❌ Не удалось пометить сообщения как прочитанные")
            );
        }, 500);
      })
      .catch((err) => console.error("Ошибка SignalR", err));

    return () => {
      isMounted = false;
      if (markAsReadTimeout) clearTimeout(markAsReadTimeout);

      connection.invoke("LeaveChat", conversationId).catch(() =>
        console.warn("❌ Не удалось покинуть чат")
      );

      connection.off("ReceiveMessage");
      connection.off("MessageStatusUpdate");
      connection.off("MessagesMarkedAsRead");
      connection.off("ChatListUpdate");
    };
  }, [params?.conversationId, params?.currentUserId]);
};

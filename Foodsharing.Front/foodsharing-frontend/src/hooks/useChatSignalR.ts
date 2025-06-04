import { useEffect } from "react";
import connection, { startConnection } from "../services/signalr-chat";

type UseChatSignalRProps = {
  conversationId: string;
  currentUserId: string;
  onNewMessage: (message: any) => void;

  onMessagesRead?: (readerId: string) => void;
  onMessageStatusUpdate?: (payload: {
    chatId: string;
    messageId: string;
    newStatus: "IsDelivered" | "IsRead";
  }) => void;
  onChatListUpdate?: (updatedChat: {
    id: string;
    interlocutor: any;
    message: any;
    unreadCount: number;
  }) => void;
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
    } = params;
    let isMounted = true;
    let markAsReadTimeout: any = null;

    startConnection()
      .then(() => {
        if (!isMounted) return;

        // 1) Подписываемся на канал конкретного чата
        connection
          .invoke("JoinChat", conversationId)
          .catch(() => console.warn("❌ Не удалось присоединиться к чату"));

        // 2) ReceiveMessage — добавляем новое сообщение к списку
        connection.on("ReceiveMessage", (message: any) => {
          onNewMessage(message);
        });

        // 3) MessageStatusUpdate — обновляем статус конкретного сообщения
        connection.on(
          "MessageStatusUpdate",
          (payload: { chatId: string; messageId: string; newStatus: string }) => {
            onMessageStatusUpdate &&
              onMessageStatusUpdate({
                chatId: payload.chatId,
                messageId: payload.messageId,
                newStatus: payload.newStatus as any,
              });
          }
        );

        // 4) MessagesMarkedAsRead — помечаем прочитано
        connection.on("MessagesMarkedAsRead", (payload: any) => {
          onMessagesRead && onMessagesRead(payload.readerId);
        });

        // 5) ChatListUpdate — пришёл апдейт карточки чата (последний текст, дата, статус, unreadCount)
        connection.on(
          "ChatListUpdate",
          (updatedChat: {
            id: string;
            interlocutor: any;
            message: any;
            unreadCount: number;
          }) => {
            onChatListUpdate && onChatListUpdate(updatedChat);
          }
        );

        // 6) Делаем «таймаут» для автоматического вызова MarkChatAsRead
        markAsReadTimeout = setTimeout(() => {
          connection
            .invoke("MarkChatAsRead", conversationId)
            .catch(() => console.warn("❌ Не удалось пометить сообщения как прочитанные"));
        }, 500);
      })
      .catch((err) => console.error("Ошибка SignalR", err));

    return () => {
      isMounted = false;
      if (markAsReadTimeout) clearTimeout(markAsReadTimeout);

      connection
        .invoke("LeaveChat", conversationId)
        .catch(() => console.warn("❌ Не удалось покинуть чат"));

      // 🧼 Очищаем обработчики SignalR во всех зарегистрированных событиях
      connection.off("ReceiveMessage");
      connection.off("MessagesMarkedAsRead");
      connection.off("MessageStatusUpdate");
      connection.off("ChatListUpdate");
    };
  }, [params?.conversationId, params?.currentUserId]);
};

// hooks/useChatSignalR.ts
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

        // 1) Присоединяемся к нужному чату
        connection.invoke("JoinChat", conversationId).catch(() =>
          console.warn("❌ Не удалось присоединиться к чату")
        );

        // 2) Прислушиваемся к новым сообщениям
        connection.on("ReceiveMessage", (message: any) => {
          onNewMessage(message);
        });

        // 3) Когда сервер пометил «прочитано» 
        connection.on("MessageStatusUpdate", (payload: any) => {
          // payload = { chatId: string, messageId: string, newStatus: "IsDelivered" | "IsRead" }
          onMessageStatusUpdate &&
            onMessageStatusUpdate({
              chatId: payload.chatId,
              messageId: payload.messageId,
              newStatus: payload.newStatus,
            });
        });

        // 4) Когда сервер говорит «все непрочитанные прочитаны»
        connection.on("MessagesMarkedAsRead", () => {
          onMessagesRead && onMessagesRead();
        });

        // 5) Когда нужно обновить карточку чата в списке
        connection.on("ChatListUpdate", (updatedChat: any) => {
          onChatListUpdate && onChatListUpdate(updatedChat);
        });

        // 6) Ждём 500 мс, а потом говорим серверу: "пометь все предыдущие сообщения как прочитанные"
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

      connection.invoke("LeaveChat", conversationId).catch(() =>
        console.warn("❌ Не удалось покинуть чат")
      );

      // Чистим все подписчики
      connection.off("ReceiveMessage");
      connection.off("MessageStatusUpdate");
      connection.off("MessagesMarkedAsRead");
      connection.off("ChatListUpdate");
    };
  }, [params?.conversationId, params?.currentUserId]);
};

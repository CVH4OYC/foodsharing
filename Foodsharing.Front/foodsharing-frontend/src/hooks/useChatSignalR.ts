import { useEffect } from "react";
import connection, { startConnection } from "../services/signalr-chat";

type UseChatSignalRProps = {
  conversationId: string;
  currentUserId: string;
  onNewMessage: (message: any) => void;
  onMessagesRead?: (readerId: string) => void;
} | null;

export const useChatSignalR = (params: UseChatSignalRProps) => {
  useEffect(() => {
    if (!params) return;

    const {
      conversationId,
      currentUserId,
      onNewMessage,
      onMessagesRead,
    } = params;

    let isMounted = true;
    let markAsReadTimeout: NodeJS.Timeout | null = null;

    const scheduleMarkAsRead = () => {
      if (markAsReadTimeout) clearTimeout(markAsReadTimeout);
      markAsReadTimeout = setTimeout(() => {
        console.log("📩 Отмечаем чат как прочитанный");
        connection
          .invoke("MarkChatAsRead", conversationId)
          .catch((err) => console.error("Ошибка при MarkChatAsRead", err));
      }, 1000);
    };

    startConnection().then(() => {
      if (!isMounted) return;

      console.log("🔌 Подключено к SignalR, чат:", conversationId);

      // 🧼 Снимаем старые подписки на всякий случай
      connection.off("ReceiveMessage");
      connection.off("MessagesMarkedAsRead");

      // 🎯 Присоединяемся к группе чата
      connection.invoke("JoinChat", conversationId).catch(console.error);

      // 📥 Обработка входящего сообщения
      connection.on("ReceiveMessage", (message) => {
        console.log("💬 Получено сообщение:", message);
        onNewMessage(message);
        scheduleMarkAsRead();
      });

      // ✅ Обработка прочтения сообщений
      connection.on("MessagesMarkedAsRead", (payload: any) => {
        const ChatId = payload?.ChatId;
        const ReaderId = payload?.ReaderId;
        console.log("📩 MessagesMarkedAsRead пришло:", ChatId, ReaderId);

        if (
          String(ChatId) === conversationId &&
          String(ReaderId) !== currentUserId
        ) {
          onMessagesRead?.(String(ReaderId));
        }
      });
    });

    return () => {
      isMounted = false;
      if (markAsReadTimeout) clearTimeout(markAsReadTimeout);

      connection
        .invoke("LeaveChat", conversationId)
        .catch(() => console.warn("❌ Не удалось покинуть чат"));

      // 🧼 Очищаем обработчики SignalR
      connection.off("ReceiveMessage");
      connection.off("MessagesMarkedAsRead");
    };
  }, [params?.conversationId, params?.currentUserId]);
};

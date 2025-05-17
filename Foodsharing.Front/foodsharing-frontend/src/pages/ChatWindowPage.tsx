import { useOutletContext, useParams, useSearchParams } from "react-router-dom";
import ChatWindow from "../components/chat/ChatWindow";

const ChatWindowPage = () => {
  const { chatId } = useParams();
  const [searchParams] = useSearchParams();
  const userId = searchParams.get("userId");

  const { onNewChatCreated } = useOutletContext<{ onNewChatCreated?: () => void }>();

  if (!chatId && !userId) {
    return <div className="p-4">Чат не найден</div>;
  }

  return (
    <ChatWindow
      chatId={chatId ?? null}
      interlocutorId={userId ?? undefined}
      onNewChatCreated={onNewChatCreated}
    />
  );
};

export default ChatWindowPage;

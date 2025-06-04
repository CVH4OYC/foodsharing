// components/chat/ChatHeader.tsx
import { FC } from "react";
import AvatarOrInitials from "./AvatarOrInitials";

interface Interlocutor {
  firstName: string;
  lastName: string;
  image: string | null;
}

interface Props {
  interlocutor: Interlocutor;
}

const ChatHeader: FC<Props> = ({ interlocutor }) => {
  const fullName = `${interlocutor.firstName} ${interlocutor.lastName}`.trim();

  return (
    <div className="border-b px-4 py-3 flex items-center gap-3">
      <AvatarOrInitials
        name={interlocutor.firstName}
        imageUrl={interlocutor.image}
        size={40}
      />
      <span className="font-semibold">{fullName}</span>
    </div>
  );
};

export default ChatHeader;

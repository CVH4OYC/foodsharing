export interface ChatDTO {
    id: string;
    interlocutor: UserDTO;
    message: MessageDTO | null;
    unreadCount: number;
  }
  
  export interface UserDTO {
    userId: string;
    userName: string;
    firstName?: string;
    lastName?: string;
    image?: string;
  }
  
  export interface ChatWithMessagesDTO {
    id: string;
    interlocutor: UserDTO;
    messages: MessageDTO[] | null;
  }
  
  export interface MessageDTO {
    id: string;
    sender: UserDTO;
    text?: string;
    date: string;
    status?: "Не прочитано" | "Доставлено" | "Прочитано";
    image?: string;
    file?: string;
  }
  
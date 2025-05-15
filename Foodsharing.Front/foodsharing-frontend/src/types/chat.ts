export interface ChatDTO {
    id: string;
    interlocutor: UserDTO;
    message: MessageDTO | null;
  }
  
  export interface UserDTO {
    userId: string;
    userName: string;
    firstName?: string;
    lastName?: string;
    image?: string;
  }
  
  export interface MessageDTO {
    isMy: boolean;
    sender: UserDTO;
    text?: string;
    date: string;
    status?: string;
    image?: string;
    file?: string;
  }
  
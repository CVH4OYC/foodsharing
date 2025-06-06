// types/transactions.ts

export interface UserDTO {
    userId: string;
    firstName: string;
    lastName: string;
    image?: string;
  }
  
  export interface OrganizationDTO {
    id: string;
    name: string;
    logoImage?: string;
  }
  
  export interface AnnouncementDTO {
    announcementId: string;
    title: string;
    image: string;
  }
  
  export interface RatingDTO {
    id: string;
    grade: number; // от 1 до 5
    comment?: string;
  }
  

  export interface TransactionDTO {
    id: string;
    sender: UserDTO;
    recipient: UserDTO;
    transactionDate: string;
    status: "Забронировано" | "Отменено" | "Завершено";
    announcement: AnnouncementDTO;
    organization?: OrganizationDTO;
    myRating?: RatingDTO;
  }
  
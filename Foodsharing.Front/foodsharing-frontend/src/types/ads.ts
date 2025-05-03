// src/types/ads.ts
export interface Category {
    id: string;
    name: string;
    parentId: string | null;
    color: string | null;
    children?: Category[];
  }
  
  export interface Announcement {
    announcementId: string;
    title: string;
    description: string;
    image: string;
    category: {
      name: string;
      color: string;
    };
    user: {
      userName: string;
      image?: string;
      firstName?: string;
      lastName?: string;
    };
    address: {
      city?: string;
      street?: string;
      house?: string;
    };
    dateCreation: string;
    expirationDate?: string;
    status?: string;
  }
  
  export interface AdCardProps extends Omit<Announcement, 'dateCreation'> {
    date?: string;
    location?: string;
    categoryName?: string;
    categoryColor?: string;
    author?: string;
    authorImage?: string;
  } 
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
      categoryId: string;
      name: string;
      color: string;
    };
    user: {
      userId: string; 
      userName: string;
      image?: string;
      firstName?: string;
      lastName?: string;
    };
    address: {
      region?: string;
      city?: string;
      street?: string;
      house?: string;
    };
    dateCreation: string;
    expirationDate?: string;
    status?: string;
    isBookedByCurrentUser: boolean;
  }
  
  export interface AdCardProps extends Omit<Announcement, 'dateCreation'> {
    date?: string;
    location?: string;
    categoryName?: string;
    categoryColor?: string;
    author?: string;
    authorImage?: string;
  } 
// src/types/ads.ts
export interface Category {
  id: string;
  name: string;
  parentId: string | null;
  color: string | null;
  children?: Category[];
}

export interface UserShort {
  userId: string;
  userName: string;
  image?: string;
  firstName?: string;
  lastName?: string;
}

export interface Address {
  region?: string;
  city?: string;
  street?: string;
  house?: string;
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
  user: UserShort;
  address: Address;
  dateCreation: string;
  expirationDate?: string;
  status?: string;
  isBookedByCurrentUser: boolean;
}

export interface UserProfile {
  userId: string;
  userName: string;
  firstName?: string;
  lastName?: string;
  bio?: string;
  image?: string;
}

export interface AdCardProps extends Omit<Announcement, 'dateCreation'> {
  date?: string;
  location?: string;
  categoryName?: string;
  categoryColor?: string;
  author?: string;
  authorImage?: string;
}
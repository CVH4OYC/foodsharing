// src/types/ads.ts
export interface OrganizationShort {
  id: string;
  name: string;
  logoImage?: string;
}

export interface Category {
  id: string;
  name: string;
  parentId: string | null;
  color: string | null;
  children?: Category[];
  isFavorite?: boolean;
}

export interface UserShort {
  userId: string;
  userName: string;
  image?: string;
  firstName?: string;
  lastName?: string;
  rating?: number;
}

export interface Address {
  region?: string;
  city?: string;
  street?: string;
  house?: string;
  latitude?: number;
  longitude?: number;
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
  user: UserShort | null; // теперь может быть null
  organization?: OrganizationShort | null; // может быть передано, если пользователь — представитель
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
  rating?: number;
}

export interface AdCardProps extends Omit<Announcement, 'dateCreation' | 'user'> {
  date?: string;
  categoryColor?: string;
  owner?: {
    name: string;
    image: string;
    link: string | null;
  };
}
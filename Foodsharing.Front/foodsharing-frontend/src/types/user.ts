export interface UserProfile {
    userId: string;
    userName: string;
    firstName?: string;
    lastName?: string;
    bio?: string;
    image?: string;
  }

export interface UserDTO {
    userId: string;
    firstName: string;
    lastName: string;
    image?: string;
  }
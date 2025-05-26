// types/org.ts

export interface OrganizationDTO {
    id: string;
    name: string;
    address?: AddressDTO | null;
    addressId?: string | null;
    phone?: string | null;
    email?: string | null;
    website?: string | null;
    description?: string | null;
    organizationForm?: string | null;
    organizationStatus?: string | null;
    logoImage?: string | null;
    isFavorite?: boolean; 
  }
  
  export interface AddressDTO {
    addressId?: string | null;
    region?: string | null;
    city?: string | null;
    street?: string | null;
    house?: string | null;
  }
  
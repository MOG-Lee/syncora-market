export type UserRole = 'Customer' | 'Vendor' | 'Admin';

export interface User {
  id: string;
  email: string;
  firstName: string;
  lastName: string;
  role: UserRole;
  token?: string; // JWT Token (usually kept separate, but sometimes here)
}

export interface LoginRequest {
    email: string;
    password: string;
}

export interface AuthResponse {
    email: string;
    token: string;
}

export interface RegisterRequest {
    firstName: string;
    lastName: string;
    email: string;
    password: string;
}


export type UserRole = 'SuperAdmin' | 'Admin' | 'User';

export interface DecodedToken {
  email: string;
  role: UserRole | UserRole[];
  exp: number;
  [key: string]: any; 
}

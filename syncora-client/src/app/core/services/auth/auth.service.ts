import { HttpClient } from '@angular/common/http';
import { inject, Injectable, signal } from '@angular/core';
import { environment } from '../../../../environments/environment';
import { AuthResponse, LoginRequest, RegisterRequest, UserRole } from '../../models/auth.model';
import { Observable, tap } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  private http = inject(HttpClient);
  private apiUrl = `${environment.identityApiUrl}/auth`;

  currentUser = signal<string | null>(null);
  currentUserRole = signal<UserRole | UserRole[] | null>(null); 

  constructor() {
    this.restoreSession();
  }

  private decodeToken(token: string): any {
    try {
      // The JWT payload is the second part of the token (index 1)
      const payload = token.split('.')[1];
      // Decode Base64
      const decodedJson = atob(payload);
      return JSON.parse(decodedJson);
    } catch (e) {
      return null;
    }
  }

  private extractAndSetRole(token: string) {
    const decoded = this.decodeToken(token);
    if (decoded) {
      const roleClaim = decoded['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'] || decoded['role'];
      this.currentUserRole.set(roleClaim);
    }
  }

  private restoreSession() {
    const token = localStorage.getItem('jwt_token');
    const email = localStorage.getItem('user_email');
    if (token && email) {
      this.currentUser.set(email);
      this.extractAndSetRole(token);
    }
  }
  // ---------------------------------

  login(credentials: LoginRequest): Observable<AuthResponse> {
    return this.http.post<AuthResponse>(`${this.apiUrl}/login`, credentials).pipe(
      tap((response) => {
        localStorage.setItem('jwt_token', response.token);
        localStorage.setItem('user_email', response.email);
        
        this.currentUser.set(response.email);
        this.extractAndSetRole(response.token); 
      })
    );
  }

  register(data: RegisterRequest): Observable<any> {
    return this.http.post(`${this.apiUrl}/register`, data, { responseType: 'text' });
  }

  logout() {
    localStorage.removeItem('jwt_token');
    localStorage.removeItem('user_email');
    this.currentUser.set(null);
    this.currentUserRole.set(null);
  }

  getToken(): string | null {
    return localStorage.getItem('jwt_token');
  }


  hasRole(allowedRoles: UserRole[]): boolean {
    const roles = this.currentUserRole();
    if (!roles) return false;

    if (Array.isArray(roles)) {
      return roles.some(r => allowedRoles.includes(r));
    }
    return allowedRoles.includes(roles);
  }
}

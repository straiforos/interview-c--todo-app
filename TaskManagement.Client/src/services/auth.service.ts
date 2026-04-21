import { BehaviorSubject, Observable, tap, from, shareReplay } from 'rxjs';
import { AuthResponse } from '../types';
import { configService } from './config.service';

class AuthService {
  private currentUserSubject = new BehaviorSubject<AuthResponse | null>(this.getStoredUser());
  public currentUser$ = this.currentUserSubject.asObservable();

  constructor() {}

  private getStoredUser(): AuthResponse | null {
    const stored = localStorage.getItem('auth_user');
    return stored ? JSON.parse(stored) : null;
  }

  public get token(): string | null {
    return this.currentUserSubject.value?.token || null;
  }

  public get isAuthenticated(): boolean {
    return !!this.token;
  }

  public login(credentials: any): Observable<AuthResponse> {
    return from(fetch(`${configService.apiUrl}/auth/login`, {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify(credentials)
    }).then(res => {
      if (!res.ok) throw new Error('Login failed');
      return res.json();
    })).pipe(
      tap((user: AuthResponse) => {
        localStorage.setItem('auth_user', JSON.stringify(user));
        this.currentUserSubject.next(user);
      }),
      shareReplay(1)
    );
  }

  public register(data: any): Observable<AuthResponse> {
    return from(fetch(`${configService.apiUrl}/auth/register`, {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify(data)
    }).then(res => {
      if (!res.ok) throw new Error('Registration failed');
      return res.json();
    })).pipe(
      tap((user: AuthResponse) => {
        localStorage.setItem('auth_user', JSON.stringify(user));
        this.currentUserSubject.next(user);
      }),
      shareReplay(1)
    );
  }

  public logout() {
    localStorage.removeItem('auth_user');
    this.currentUserSubject.next(null);
  }
}

export const authService = new AuthService();

import { BehaviorSubject, Observable, tap, from, shareReplay } from 'rxjs';
import { AuthResponse } from '../types';
import { configService } from './config.service';
import { apiClient } from './api.client';

/**
 * Service responsible for managing user authentication, roles, and permissions.
 * Uses RxJS Observables to broadcast the current user state to the application.
 */
class AuthService {
  private currentUserSubject = new BehaviorSubject<AuthResponse | null>(this.getStoredUser());
  
  /**
   * Observable stream of the current authenticated user.
   * Emits `null` when the user is logged out.
   */
  public currentUser$ = this.currentUserSubject.asObservable();

  constructor() {
    window.addEventListener('unauthorized', () => {
      this.logout();
      window.location.href = '/login';
    });
  }

  /**
   * Retrieves the stored user data from local storage.
   * @returns The stored AuthResponse or null if not found.
   */
  private getStoredUser(): AuthResponse | null {
    const stored = localStorage.getItem('auth_user');
    return stored ? JSON.parse(stored) : null;
  }

  /**
   * Gets the current JWT access token.
   * @returns The JWT token or null if not authenticated.
   */
  public get token(): string | null {
    return this.currentUserSubject.value?.token || null;
  }

  /**
   * Checks if a user is currently authenticated.
   * @returns True if a valid token exists.
   */
  public get isAuthenticated(): boolean {
    return !!this.token;
  }

  /**
   * Checks if the current user has a specific role.
   * @param role The role name to check (e.g., 'Admin', 'User').
   * @returns True if the user has the role.
   */
  public hasRole(role: string): boolean {
    return this.currentUserSubject.value?.role === role;
  }

  /**
   * Checks if the current user has a specific permission.
   * @param permission The permission name to check (e.g., 'Tasks.Create').
   * @returns True if the user's role grants the permission.
   */
  public hasPermission(permission: string): boolean {
    return this.currentUserSubject.value?.permissions?.includes(permission) ?? false;
  }

  /**
   * Authenticates a user with the backend API.
   * @param credentials The login credentials (email and password).
   * @returns An Observable containing the AuthResponse.
   */
  public login(credentials: any): Observable<AuthResponse> {
    return from(apiClient(`${configService.apiUrl}/auth/login`, {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify(credentials)
    }).then(async res => {
      if (!res.ok) {
        const errorText = await res.text();
        console.error('Login failed:', res.status, errorText);
        throw new Error('Login failed');
      }
      return res.json();
    })).pipe(
      tap((user: AuthResponse) => {
        localStorage.setItem('auth_user', JSON.stringify(user));
        this.currentUserSubject.next(user);
      }),
      shareReplay(1)
    );
  }

  /**
   * Registers a new user with the backend API.
   * @param data The registration data (email, username, password).
   * @returns An Observable containing the AuthResponse.
   */
  public register(data: any): Observable<AuthResponse> {
    return from(apiClient(`${configService.apiUrl}/auth/register`, {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify(data)
    }).then(async res => {
      if (!res.ok) {
        const errorText = await res.text();
        console.error('Registration failed:', res.status, errorText);
        throw new Error('Registration failed');
      }
      return res.json();
    })).pipe(
      tap((user: AuthResponse) => {
        localStorage.setItem('auth_user', JSON.stringify(user));
        this.currentUserSubject.next(user);
      }),
      shareReplay(1)
    );
  }

  /**
   * Swaps the current user's role (Demo feature).
   * @param roleName The name of the role to swap to.
   * @returns An Observable containing the updated AuthResponse.
   */
  public swapRole(roleName: string): Observable<AuthResponse> {
    return from(apiClient(`${configService.apiUrl}/auth/swap-role`, {
      method: 'POST',
      headers: { 
        'Content-Type': 'application/json',
        'Authorization': `Bearer ${this.token}`
      },
      body: JSON.stringify({ roleName })
    }).then(async res => {
      if (!res.ok) {
        const errorText = await res.text();
        console.error('Role swap failed:', res.status, errorText);
        throw new Error('Role swap failed');
      }
      return res.json();
    })).pipe(
      tap((user: AuthResponse) => {
        localStorage.setItem('auth_user', JSON.stringify(user));
        this.currentUserSubject.next(user);
      }),
      shareReplay(1)
    );
  }

  /**
   * Logs out the current user by clearing local storage and emitting null.
   */
  public logout() {
    localStorage.removeItem('auth_user');
    this.currentUserSubject.next(null);
  }
}

/**
 * Singleton instance of the AuthService.
 */
export const authService = new AuthService();

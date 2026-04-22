/**
 * Response returned after a successful authentication (login/register).
 */
export interface AuthResponse {
  /** JWT access token */
  token: string;
  /** User's email address */
  email: string;
  /** Unique identifier for the user */
  userId: string;
  /** User's role */
  role: string;
  /** User's permissions */
  permissions: string[];
}

/**
 * DTO representing a user in the system.
 */
export interface UserDto {
  /** Unique identifier for the user */
  id: string;
  /** User's email address */
  email: string;
}

/**
 * Runtime configuration for the frontend application.
 */
export interface AppConfig {
  /** The base URL for the backend API */
  apiUrl: string;
  /** Optional feature flags */
  features?: {
    [key: string]: boolean | undefined;
  };
}

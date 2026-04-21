/**
 * Runtime configuration for the frontend application.
 */
export interface AppConfig {
  /** The base URL for the backend API */
  apiUrl: string;
  /** The base URL for the real-time notification hub */
  wsUrl: string;
  /** Optional feature flags */
  features?: {
    enableNotifications?: boolean;
    [key: string]: boolean | undefined;
  };
}

import { BehaviorSubject, firstValueFrom } from 'rxjs';
import { AppConfig } from '../types';

class ConfigService {
  private configSubject = new BehaviorSubject<AppConfig | null>(null);
  public config$ = this.configSubject.asObservable();

  /**
   * Loads the configuration from the external /config.json file.
   * This should be called and awaited before the application renders.
   */
  public async loadConfig(): Promise<AppConfig> {
    try {
      const response = await fetch('/config.json');
      if (!response.ok) {
        throw new Error(`Failed to load config.json: ${response.statusText}`);
      }
      const config = await response.json();
      this.configSubject.next(config);
      return config;
    } catch (error) {
      console.error('Error loading runtime configuration:', error);
      // Fallback for local development if config.json is missing
      const fallback: AppConfig = {
        apiUrl: 'http://localhost:5000/api',
        wsUrl: 'http://localhost:5000/hubs/notifications'
      };
      this.configSubject.next(fallback);
      return fallback;
    }
  }

  public get apiUrl(): string {
    return this.configSubject.value?.apiUrl || 'http://localhost:5000/api';
  }

  public get wsUrl(): string {
    return this.configSubject.value?.wsUrl || 'http://localhost:5000/hubs/notifications';
  }

  public isFeatureEnabled(featureKey: string): boolean {
    return !!this.configSubject.value?.features?.[featureKey];
  }
}

export const configService = new ConfigService();

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
      throw error;
    }
  }

  private get config(): AppConfig {
    if (!this.configSubject.value) {
      throw new Error('Configuration not loaded. Ensure configService.loadConfig() is called and awaited at application startup.');
    }
    return this.configSubject.value;
  }

  public get apiUrl(): string {
    return this.config.apiUrl;
  }

  public get wsUrl(): string {
    return this.config.wsUrl;
  }

  public isFeatureEnabled(featureKey: string): boolean {
    return !!this.config.features?.[featureKey];
  }
}

export const configService = new ConfigService();

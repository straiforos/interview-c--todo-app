import { i18n } from '@lingui/core';
import { BehaviorSubject } from 'rxjs';

/**
 * Service to manage internationalization and locale switching.
 */
class I18nService {
  private localeSubject = new BehaviorSubject<string>(this.getInitialLocale());
  public locale$ = this.localeSubject.asObservable();

  constructor() {}

  private getInitialLocale(): string {
    return localStorage.getItem('preferred_locale') || 'en';
  }

  /**
   * Dynamically loads and activates a locale.
   * @param locale The locale code to activate (e.g., 'en', 'es').
   */
  public async activate(locale: string) {
    try {
      const { messages } = await import(`../locales/${locale}/messages.json`);
      i18n.load(locale, messages);
      i18n.activate(locale);
      this.localeSubject.next(locale);
      localStorage.setItem('preferred_locale', locale);
    } catch (error) {
      console.error(`Failed to load messages for locale: ${locale}`, error);
    }
  }

  /**
   * Initializes the i18n system with the preferred locale.
   */
  public async init() {
    await this.activate(this.localeSubject.value);
  }
}

export const i18nService = new I18nService();

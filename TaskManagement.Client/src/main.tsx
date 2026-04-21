import React from 'react';
import ReactDOM from 'react-dom/client';
import App from './App';
import './index.css';
import { TooltipProvider } from '@/components/ui/tooltip';
import { configService } from './services/config.service';
import { i18nService } from './services/i18n.service';
import { i18n } from '@lingui/core';
import { I18nProvider } from '@lingui/react';

async function init() {
  // Load runtime configuration and i18n before rendering (ADR 0024, ADR 0023)
  await Promise.all([
    configService.loadConfig(),
    i18nService.init()
  ]);

  ReactDOM.createRoot(document.getElementById('root')!).render(
    <React.StrictMode>
      <I18nProvider i18n={i18n}>
        <TooltipProvider>
          <App />
        </TooltipProvider>
      </I18nProvider>
    </React.StrictMode>
  );
}

init();


import React from 'react';
import ReactDOM from 'react-dom/client';
import App from './App';
import './index.css';
import { TooltipProvider } from '@/components/ui/tooltip';
import { configService } from './services/config.service';

async function init() {
  // Load runtime configuration before rendering (ADR 0024)
  await configService.loadConfig();

  ReactDOM.createRoot(document.getElementById('root')!).render(
    <React.StrictMode>
      <TooltipProvider>
        <App />
      </TooltipProvider>
    </React.StrictMode>
  );
}

init();

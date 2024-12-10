import { ApplicationConfig, provideZoneChangeDetection } from '@angular/core';
import { provideRouter } from '@angular/router';
import { provideStoreDevtools } from '@ngrx/store-devtools';
import { routes } from './app.routes';
import { provideHttpClient, withInterceptors } from '@angular/common/http';
import { httpRequestInterceptor } from './core/http.interceptor';

export const appConfig: ApplicationConfig = {
  providers: [
    provideHttpClient(withInterceptors([httpRequestInterceptor])),
    provideStoreDevtools({
      maxAge: 25, // Retains last 25 states
      logOnly: true, // Restrict extension to log-only mode in production
    }),
    provideZoneChangeDetection({ eventCoalescing: true }),
    provideRouter(routes)],
};

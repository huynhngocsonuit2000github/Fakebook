import { Routes } from '@angular/router';

export const routes: Routes = [
    // Login Route
    {
        path: 'login',
        loadComponent: () => import('./pages/login/login.component').then(m => m.LoginComponent)
    },
];

import { Routes } from '@angular/router';
import { MemberLayoutComponent } from './layouts/member-layout/member-layout.component';
import { AdminLayoutComponent } from './layouts/admin-layout/admin-layout.component';

export const routes: Routes = [
    // Member Routes
    {
        path: '',
        component: AdminLayoutComponent,
        children: [
            {
                path: 'cases',
                loadComponent: () => import('./pages/cases/cases.component').then(m => m.CasesComponent),
            },
            {
                path: 'sets',
                loadComponent: () => import('./pages/sets/sets.component').then(m => m.SetsComponent),
            },
            {
                path: 'cycles',
                loadComponent: () => import('./pages/cycles/cycles.component').then(m => m.CyclesComponent),
            },
            {
                path: 'pipelines',
                loadComponent: () => import('./pages/pipelines/pipelines.component').then(m => m.PipelinesComponent),
            },
            { path: '', redirectTo: 'cases', pathMatch: 'full' } // Default navigation to "cases"
        ]
    },

    // Fallback Route (If the route does not match any, redirect to cases)
    { path: '**', redirectTo: 'cases', pathMatch: 'full' },
];

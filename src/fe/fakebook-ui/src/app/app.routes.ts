import { Routes } from '@angular/router';
import { PermissionGuard } from './guards/permission.guard';
import { UnauthorizedComponent } from './pages/unauthorized/unauthorized.component';
import { MemberLayoutComponent } from './layouts/member-layout/member-layout.component';
import { AdminLayoutComponent } from './layouts/admin-layout/admin-layout.component';

export const routes: Routes = [
    // Member Routes
    {
        path: '',
        component: MemberLayoutComponent,
        children: [
            {
                path: '',
                loadComponent: () => import('./pages/home/home.component').then(m => m.HomeComponent),
                canActivate: [PermissionGuard],
                data: { permission: 'member_read' },
            },
            {
                path: 'friends',
                loadComponent: () => import('./pages/friends/friends.component').then(m => m.FriendsComponent),
                canActivate: [PermissionGuard],
                data: { permission: 'member_create' },
            },
        ]
    },

    // Admin Routes
    {
        path: 'admin',
        component: AdminLayoutComponent,
        children: [
            {
                path: 'dashboard',
                loadComponent: () => import('./pages/dashboard/dashboard.component').then(m => m.DashboardComponent),
                canActivate: [PermissionGuard],
                data: { permission: 'admin_read' },
            },
            {
                path: 'user-management',
                loadComponent: () => import('./pages/user-management/user-management.component').then(m => m.UserManagementComponent),
                canActivate: [PermissionGuard],
                data: { permission: 'admin_create' },
            },
            { path: '', redirectTo: 'dashboard', pathMatch: 'full' }
        ]
    },

    // Login Route
    {
        path: 'callback',
        loadComponent: () => import('./compoments/callback/callback.component').then(m => m.CallbackComponent)
    },

    // Unauthorized Route
    {
        path: 'unauthorized',
        component: UnauthorizedComponent,
    },

    // Login Route
    {
        path: 'login',
        loadComponent: () => import('./pages/login/login.component').then(m => m.LoginComponent)
    },

    // Fallback Route (If the route does not match any, redirect to home page or another path)
    { path: '', redirectTo: '/', pathMatch: 'full' }
];

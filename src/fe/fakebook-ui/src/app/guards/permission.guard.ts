import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, Router } from '@angular/router';
import { PermissionService } from '../services/permission.service';
import { map, Observable } from 'rxjs';

@Injectable({
    providedIn: 'root',
})
export class PermissionGuard implements CanActivate {
    constructor(private permissionService: PermissionService, private router: Router) { }

    canActivate(route: ActivatedRouteSnapshot): Observable<boolean> {
        const requiredPermission = route.data['permission'];

        // Use hasPermission and return the observable directly
        return this.permissionService.hasPermission(requiredPermission).pipe(
            map((authenAutho) => {
                if (!authenAutho.isAuth) {
                    this.router.navigate(['/unauthorized']);
                    return false;
                } else {
                    if (authenAutho.isAuth && !authenAutho.validPermission) {
                        this.router.navigate(['/forbidden']);
                        return false;
                    }

                    return true;
                }
            })
        );
    }
}

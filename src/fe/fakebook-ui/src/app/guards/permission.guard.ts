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
        console.log('Required permission:', requiredPermission);

        // Use hasPermission and return the observable directly
        return this.permissionService.hasPermission(requiredPermission).pipe(
            map((hasPermission) => {
                if (hasPermission) {
                    return true;
                } else {
                    // Redirect to unauthorized page if permission is not granted
                    this.router.navigate(['/unauthorized']);
                    return false;
                }
            })
        );
    }
}

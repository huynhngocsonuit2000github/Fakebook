import { Injectable } from '@angular/core';
import { AuthUser } from '../models/AuthUser';
import { AccountState } from '../state/account.state';
import { Store } from '@ngrx/store';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class PermissionService {
  private currentUser: AuthUser | null = null;

  constructor(
    private store: Store<{ account: AccountState }>) {
    this.store.select(state => state.account.authUser).subscribe(authUser => {
      this.currentUser = authUser;
    });
  }

  // Check if the user has a specific permission
  isAuthenticated(): Observable<boolean> {
    return new Observable<boolean>((observer) => {
      if (!this.currentUser) {
        observer.next(false);
      } else {
        observer.next(this.currentUser.isAuthenticated);
      }
    });
  }

  // Check if the user has a specific permission
  hasPermission(permissionName: string): Observable<UserCredential> {
    return new Observable<UserCredential>((observer) => {
      if (!this.currentUser || !this.currentUser.isAuthenticated) {
        // User is not authenticated
        observer.next({ isAuth: false, validPermission: false });
      } else {
        // Check for required permission
        const validPermission = this.currentUser.userPermissions.includes(permissionName);
        observer.next({ isAuth: true, validPermission });
      }
      observer.complete();
    });
  }
}

export interface UserCredential {
  isAuth: boolean;
  validPermission: boolean
}
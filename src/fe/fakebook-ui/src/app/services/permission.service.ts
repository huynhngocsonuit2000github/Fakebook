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
  hasPermission(permissionName: string): Observable<boolean> {
    return new Observable<boolean>((observer) => {
      if (!this.currentUser) {
        observer.next(false);
      } else {
        // Assuming userPermissions is an array
        const hasPermission = this.currentUser.userPermissions.some(
          (permission) => permission === permissionName
        );
        observer.next(hasPermission);
      }
    });
  }
}

import { Component } from '@angular/core';
import { Store } from '@ngrx/store';
import { Observable } from 'rxjs';
import { AccountState } from '../../state/account.state';
import { loginRequest, logOut } from '../../state/actions/account.actions';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-test-account',
  standalone: true,
  imports: [CommonModule],
  template: `
    <div>
      <h2>Account Information</h2>
      <div *ngIf="account$ | async as account">
        <p><strong>Token:</strong> {{ account.token || 'No token available' }}</p>
        <p><strong>Loading:</strong> {{ account.loading }}</p>
        <p><strong>Error:</strong> {{ account.error || 'No errors' }}</p>
      </div>
      <hr />
      <button (click)="simulateLogin()">Simulate Login</button>
      <button (click)="simulateLogout()">Simulate Logout</button>
    </div>
  `,
})
export class TestAccountComponent {
  account$: Observable<AccountState>;

  constructor(private store: Store<{ account: AccountState }>) {
    // Select the 'account' slice of state from the store
    this.account$ = this.store.select('account');
  }

  // Dispatch a loginRequest action with mock credentials
  simulateLogin() {
    console.log('Component: Dispatching loginRequest action...');
    this.store.dispatch(
      loginRequest({ username: 'proustest1', password: 'propwtest1' })
    );
  }

  // Dispatch a logOut action
  simulateLogout() {
    console.log('Component: Dispatching logOut action...');
    this.store.dispatch(logOut());
  }
}

import { Component } from '@angular/core';
import { Store } from '@ngrx/store';
import { Observable } from 'rxjs';
import { AccountState } from '../../state/account.state';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-account-info',
  standalone: true,
  imports: [CommonModule],
  template: `
    <div *ngIf="account$ | async as account">
      <p><strong>From new component Token:</strong> {{ account.token || 'No token' }}</p>
      <p><strong>From new component Loading:</strong> {{ account.loading }}</p>
      <p><strong>From new component Error:</strong> {{ account.error || 'No error' }}</p>
    </div>
  `,
})
export class AccountInfoComponent {
  account$: Observable<AccountState>;

  constructor(private store: Store<{ account: AccountState }>) {
    this.account$ = this.store.select('account');
  }
}

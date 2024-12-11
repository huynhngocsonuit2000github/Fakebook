import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router, ActivatedRoute } from '@angular/router';
import { Store } from '@ngrx/store';
import { AccountState } from '../../state/account.state';
import { loginRequest, logOut } from '../../state/actions/account.actions';
import { Observable } from 'rxjs';
import { environment } from './../../../environments/environment';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './login.component.html',
  styleUrl: './login.component.scss'
})
export class LoginComponent implements OnInit {
  loginForm: FormGroup;
  submitted = false;
  isFingerprintOpen = false;

  authError$: Observable<string | null>;
  loading$: Observable<boolean>;

  constructor(
    private formBuilder: FormBuilder,
    private router: Router,
    private route: ActivatedRoute,
    private store: Store<{ account: AccountState }>
  ) {
    this.loginForm = this.formBuilder.group({
      username: ['', Validators.required],
      password: ['', Validators.required],
    });

    this.authError$ = this.store.select(state => state.account.error);
    this.loading$ = this.store.select(state => state.account.loading);
  }

  ngOnInit(): void {
    // Logout user or initialize the component
    this.logout();
  }

  get f() {
    return this.loginForm.controls as { [key: string]: any };
  }

  onSubmit(): void {
    this.submitted = true;

    if (this.loginForm.valid) {
      const { username, password } = this.loginForm.value;
      const returnUrl = this.route.snapshot.queryParams['returnUrl'] || '/';

      this.store.dispatch(loginRequest({ username, password }));
    }
  }

  logout(): void {
    this.store.dispatch(logOut());
  }

  openFingerprintModal(): void {
    this.isFingerprintOpen = true;
  }

  closeFingerprintModal(): void {
    this.isFingerprintOpen = false;
  }

  loginWithIdP() {
    const returnUrl = 'callback'; // You can dynamically construct this if needed
    const idpLoginUrl = `${environment.idpUIUrl}/login?returnUrl=${encodeURIComponent(returnUrl)}`;
    window.location.href = idpLoginUrl;
  }
}
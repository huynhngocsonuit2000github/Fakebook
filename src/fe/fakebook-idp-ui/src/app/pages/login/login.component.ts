import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router, ActivatedRoute } from '@angular/router';
import { Store } from '@ngrx/store';
import { Observable } from 'rxjs';
import { UserService } from '../../services/user.service';
import { LoginResponse } from '../../models/LoginResponse';

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

  constructor(
    private formBuilder: FormBuilder,
    private router: Router,
    private route: ActivatedRoute,
    private userService: UserService,
  ) {
    this.loginForm = this.formBuilder.group({
      username: ['', Validators.required],
      password: ['', Validators.required],
    });
  }


  get f() {
    return this.loginForm.controls as { [key: string]: any };
  }

  ngOnInit(): void {
  }

  onSubmit(): void {
    this.submitted = true;

    if (this.loginForm.valid) {
      const { username, password } = this.loginForm.value;
      let returnUrl = this.route.snapshot.queryParams['returnUrl'] || '/';

      console.log('Input:', username, password, returnUrl);

      this.userService.login(username, password)
        .subscribe((res: LoginResponse) => {
          const email = res.email;
          const token = res.token;

          if (email) {
            returnUrl = 'http://localhost:4200/'
            window.location.href = `${returnUrl}?email=${email}&token=${token}`;
          }
        },
          error => {
            console.log('Login failed:', error.message);
          })
    }
  }
}
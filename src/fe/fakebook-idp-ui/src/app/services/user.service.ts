// src/app/services/user.service.ts
import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from './../../environments/environment';
import { LoginResponse } from '../models/LoginResponse';

@Injectable({
  providedIn: 'root',
})
export class UserService {
  private apiUrl = `${environment.apiUrl}/auth`;

  constructor(private http: HttpClient) { }

  login(username: string, password: string): Observable<LoginResponse> {
    console.log(this.apiUrl);

    const headers = new HttpHeaders({ 'Content-Type': 'application/json' });
    const body = { username, password };

    return this.http.post(`${this.apiUrl}/login`, body, { headers }) as Observable<LoginResponse>;
  }
}

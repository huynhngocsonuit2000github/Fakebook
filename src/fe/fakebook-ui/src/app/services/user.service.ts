// src/app/services/user.service.ts
import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpErrorResponse } from '@angular/common/http';
import { Observable, of, throwError } from 'rxjs';
import { catchError, map } from 'rxjs/operators';
import { environment } from './../../environments/environment';

@Injectable({
  providedIn: 'root',
})
export class UserService {
  // private apiUrl = `http://192.168.50.10:32000/user`; // Use environment variable for API URL
  private apiUrl = `${environment.apiUrl}`; // Use environment variable for API URL

  constructor(private http: HttpClient) { }

  exchangeIdPToken(email: string, idPToken: string): Observable<string> {
    const headers = new HttpHeaders({ 'Content-Type': 'application/json' });
    const body = { email, idPToken };

    return this.http
      .post(`${this.apiUrl}/auth/exchange-idp-token`, body, { headers, responseType: 'text' })
      .pipe(
        map((response: string) => {
          sessionStorage.setItem('token', response); // Store token in sessionStorage
          return response;
        }),
        catchError(this.handleError) // Handle errors
      );
  }

  getUserPermissions(token: string): Observable<string[]> {
    const headers = new HttpHeaders({
      'Authorization': `Bearer ${token}` // Add Bearer token to the headers
    });

    console.log('token', token);
    console.log('Headers:', headers);


    // Remove 'responseType: 'text'' and let Angular handle the response as JSON
    return this.http.get<string[]>(`${this.apiUrl}/auth/get-user-permission`, {
      headers,
    });
  }


  login(username: string, password: string): Observable<string> {
    const headers = new HttpHeaders({ 'Content-Type': 'application/json' });
    const body = { username, password };

    return this.http
      .post(`${this.apiUrl}/auth/login`, body, { headers, responseType: 'text' })
      .pipe(
        map((response: string) => {
          sessionStorage.setItem('token', response); // Store token in sessionStorage
          return response;
        }),
        catchError(this.handleError) // Handle errors
      );
  }

  logout(): void {
    sessionStorage.removeItem('user'); // Remove user info from session storage
  }

  private handleError(error: HttpErrorResponse): Observable<never> {
    const errorMsg = error.error || error.statusText || 'Unknown error occurred';
    return throwError(errorMsg); // Return an observable with the error message
  }
}

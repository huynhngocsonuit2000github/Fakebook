// src/app/services/user.service.ts
import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpErrorResponse } from '@angular/common/http';
import { Observable, of, throwError } from 'rxjs';
import { catchError, map } from 'rxjs/operators';

@Injectable({
  providedIn: 'root',
})
export class UserService {
  private apiUrl = `http://192.168.50.10:32000/user`; // Use environment variable for API URL

  constructor(private http: HttpClient) { }

  login(username: string, password: string): Observable<string> {
    console.log('Service: execute login');

    const headers = new HttpHeaders({ 'Content-Type': 'application/json' });
    const body = { username, password };

    return this.http
      .post(`${this.apiUrl}/login`, body, { headers, responseType: 'text' })
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
    console.log('res', error);

    if (error.status === 401) {
      this.logout(); // Log out if unauthorized
    }

    const errorMsg = error.error || error.statusText || 'Unknown error occurred';
    return throwError(errorMsg); // Return an observable with the error message
  }
}

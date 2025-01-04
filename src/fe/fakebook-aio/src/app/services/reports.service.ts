import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { HttpClient, HttpHeaders, HttpErrorResponse } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ReportsService {

  private apiUrl = `${environment.apiUrl}`; // Use environment variable for API URL

  constructor(private http: HttpClient) { }

  triggerCases(caseId: string): Observable<any> {
    const headers = new HttpHeaders({ 'Content-Type': 'application/json' });

    return this.http
      .get<any>(`${this.apiUrl}/api/reports/trigger-job/${caseId}`);
  }
}

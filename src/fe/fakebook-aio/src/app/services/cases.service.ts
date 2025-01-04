import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { HttpClient, HttpHeaders, HttpErrorResponse } from '@angular/common/http';
import { Observable } from 'rxjs';
import { CaseModel, CaseCreateModel, CaseUpdateModel } from './models/cases.model';

@Injectable({
  providedIn: 'root'
})
export class CasesService {

  private apiUrl = `${environment.apiUrl}`; // Use environment variable for API URL

  constructor(private http: HttpClient) { }

  getAllCases(): Observable<CaseModel[]> {
    const headers = new HttpHeaders({ 'Content-Type': 'application/json' });

    return this.http
      .get<CaseModel[]>(`${this.apiUrl}/api/cases`, { headers });
  }

  createCases(input: CaseCreateModel): Observable<CaseModel> {
    const headers = new HttpHeaders({ 'Content-Type': 'application/json' });

    return this.http
      .post<CaseModel>(`${this.apiUrl}/api/cases`, input, { headers });
  }

  updateCases(id: string, input: CaseUpdateModel): Observable<any> {
    const headers = new HttpHeaders({ 'Content-Type': 'application/json' });

    return this.http
      .patch<any>(`${this.apiUrl}/api/cases/${id}`, input, { headers });
  }

  deleteCases(id: string): Observable<any> {
    const headers = new HttpHeaders({ 'Content-Type': 'application/json' });

    return this.http
      .delete<any>(`${this.apiUrl}/api/cases/${id}`, { headers });
  }
}

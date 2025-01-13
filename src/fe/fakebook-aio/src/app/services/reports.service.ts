import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { HttpClient, HttpHeaders, HttpErrorResponse } from '@angular/common/http';
import { Observable } from 'rxjs';
import { PipelineCreateModel, PipelineModel } from './models/pipeline.model';

@Injectable({
  providedIn: 'root'
})
export class ReportsService {

  private apiUrl = `${environment.apiUrl}`; // Use environment variable for API URL

  constructor(private http: HttpClient) { }


  getAllPipelines(): Observable<PipelineModel[]> {
    const headers = new HttpHeaders({ 'Content-Type': 'application/json' });

    return this.http
      .get<PipelineModel[]>(`${this.apiUrl}/api/reports`, { headers });
  }

  createJob(input: PipelineCreateModel): Observable<any> {
    const headers = new HttpHeaders({ 'Content-Type': 'application/json' });

    return this.http
      .post<any>(`${this.apiUrl}/api/reports/create-job`, input, { headers });
  }

  updateJob(id: string, input: PipelineCreateModel): Observable<any> {
    const headers = new HttpHeaders({ 'Content-Type': 'application/json' });

    return this.http
      .post<any>(`${this.apiUrl}/api/reports/update-job/${id}`, input, { headers });
  }

  triggerCases(caseId: string, pipelineId: string): Observable<any> {
    const headers = new HttpHeaders({ 'Content-Type': 'application/json' });

    return this.http
      .get<any>(`${this.apiUrl}/api/reports/trigger-job?caseId=${caseId}&pipelineId=${pipelineId}`);
  }
}

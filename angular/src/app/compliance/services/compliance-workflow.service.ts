import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { WorkflowActionHistoryDto } from '../models/compliance.models';

@Injectable({ providedIn: 'root' })
export class ComplianceWorkflowService {
  private readonly baseUrl = `${environment.apis.default.url}/api/compliance/templates`;

  constructor(private http: HttpClient) {}

  submit(templateId: string): Observable<void> {
    return this.http.post<void>(`${this.baseUrl}/${templateId}/submit`, {});
  }

  approve(templateId: string): Observable<void> {
    return this.http.post<void>(`${this.baseUrl}/${templateId}/approve`, {});
  }

  return(templateId: string, comment: string): Observable<void> {
    return this.http.post<void>(`${this.baseUrl}/${templateId}/return`, { comment });
  }

  resubmit(templateId: string): Observable<void> {
    return this.http.post<void>(`${this.baseUrl}/${templateId}/resubmit`, {});
  }

  archive(templateId: string): Observable<void> {
    return this.http.post<void>(`${this.baseUrl}/${templateId}/archive`, {});
  }

  getHistory(templateId: string): Observable<WorkflowActionHistoryDto[]> {
    return this.http.get<WorkflowActionHistoryDto[]>(`${this.baseUrl}/${templateId}/history`);
  }
}

import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { ChecklistItemDto, CreateUpdateItemInput } from '../models/compliance.models';

@Injectable({ providedIn: 'root' })
export class ComplianceItemsService {
  private readonly baseUrl = `${environment.apis.default.url}/api/compliance`;

  constructor(private http: HttpClient) {}

  create(templateId: string, input: CreateUpdateItemInput): Observable<ChecklistItemDto> {
    return this.http.post<ChecklistItemDto>(
      `${this.baseUrl}/templates/${templateId}/items`,
      input
    );
  }

  update(id: string, input: CreateUpdateItemInput): Observable<ChecklistItemDto> {
    return this.http.put<ChecklistItemDto>(`${this.baseUrl}/items/${id}`, input);
  }

  delete(id: string): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/items/${id}`);
  }
}

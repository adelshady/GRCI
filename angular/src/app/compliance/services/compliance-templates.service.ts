import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import {
  ChecklistTemplateDto,
  ChecklistItemDto,
  CreateUpdateTemplateInput,
  ListTemplatesInput,
  PagedResult,
} from '../models/compliance.models';

@Injectable({ providedIn: 'root' })
export class ComplianceTemplatesService {
  private readonly baseUrl = `${environment.apis.default.url}/api/compliance/templates`;

  constructor(private http: HttpClient) {}

  getList(input: ListTemplatesInput = {}): Observable<PagedResult<ChecklistTemplateDto>> {
    let params = new HttpParams();
    if (input.search) params = params.set('search', input.search);
    if (input.status !== undefined && input.status !== null)
      params = params.set('status', input.status.toString());
    if (input.regulatoryAgencyId) params = params.set('regulatoryAgencyId', input.regulatoryAgencyId);
    if (input.skipCount !== undefined) params = params.set('skipCount', input.skipCount.toString());
    if (input.maxResultCount !== undefined)
      params = params.set('maxResultCount', input.maxResultCount.toString());
    return this.http.get<PagedResult<ChecklistTemplateDto>>(this.baseUrl, { params });
  }

  get(id: string): Observable<ChecklistTemplateDto> {
    return this.http.get<ChecklistTemplateDto>(`${this.baseUrl}/${id}`);
  }

  create(input: CreateUpdateTemplateInput): Observable<ChecklistTemplateDto> {
    return this.http.post<ChecklistTemplateDto>(this.baseUrl, input);
  }

  update(id: string, input: CreateUpdateTemplateInput): Observable<ChecklistTemplateDto> {
    return this.http.put<ChecklistTemplateDto>(`${this.baseUrl}/${id}`, input);
  }

  delete(id: string): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/${id}`);
  }

  getItems(templateId: string): Observable<ChecklistItemDto[]> {
    return this.http.get<ChecklistItemDto[]>(`${this.baseUrl}/${templateId}/items`);
  }
}

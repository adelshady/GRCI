import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { AttachmentDto } from '../models/compliance.models';

@Injectable({ providedIn: 'root' })
export class ComplianceAttachmentsService {
  private readonly baseUrl = `${environment.apis.default.url}/api/compliance`;

  constructor(private http: HttpClient) {}

  getAttachments(itemId: string): Observable<AttachmentDto[]> {
    return this.http.get<AttachmentDto[]>(`${this.baseUrl}/items/${itemId}/attachments`);
  }

  upload(itemId: string, file: File): Observable<AttachmentDto> {
    const formData = new FormData();
    formData.append('file', file);
    return this.http.post<AttachmentDto>(`${this.baseUrl}/items/${itemId}/attachments`, formData);
  }

  download(fileId: string): Observable<Blob> {
    return this.http.get(`${this.baseUrl}/attachments/${fileId}/download`, {
      responseType: 'blob',
    });
  }

  delete(fileId: string): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/attachments/${fileId}`);
  }
}

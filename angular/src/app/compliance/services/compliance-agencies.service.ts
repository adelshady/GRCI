import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { RegulatoryAgencyDto } from '../models/compliance.models';
import { environment } from '../../../environments/environment';

@Injectable({ providedIn: 'root' })
export class ComplianceAgenciesService {
  private readonly baseUrl = `${environment.apis.default.url}/api/compliance/agencies`;

  constructor(private http: HttpClient) {}

  getList(activeOnly = true): Observable<RegulatoryAgencyDto[]> {
    return this.http.get<RegulatoryAgencyDto[]>(this.baseUrl, {
      params: { activeOnly: String(activeOnly) },
    });
  }
}

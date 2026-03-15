import { AuthService } from '@abp/ng.core';
import { Component } from '@angular/core';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss'],
})
export class HomeComponent {
  get hasLoggedIn(): boolean {
    return this.authService.isAuthenticated;
  }

  constructor(private authService: AuthService) {}

  login() {
    this.authService.navigateToLogin();
  }

  features = [
    { icon: 'fa fa-file-text-o', title: 'Template Management', desc: 'Create & version compliance checklists', color: 'var(--g-primary)', bg: 'rgba(129,140,248,.12)' },
    { icon: 'fa fa-check-circle', title: 'Workflow Approvals', desc: 'Submit, review, and approve templates', color: 'var(--g-success)', bg: 'var(--g-success-bg)' },
    { icon: 'fa fa-history', title: 'Audit Trail', desc: 'Full history of all changes and actions', color: 'var(--g-cyan)', bg: 'var(--g-info-bg)' },
    { icon: 'fa fa-language', title: 'Bilingual Support', desc: 'Arabic & English content throughout', color: 'var(--g-purple)', bg: 'rgba(167,139,250,.12)' },
    { icon: 'fa fa-paperclip', title: 'Attachments', desc: 'Attach evidence files to checklist items', color: 'var(--g-warning)', bg: 'var(--g-warning-bg)' },
    { icon: 'fa fa-lock', title: 'Role-Based Access', desc: 'Granular permissions per user role', color: 'var(--g-danger)', bg: 'var(--g-danger-bg)' },
  ];

  quickLinks = [
    { route: '/compliance/templates', icon: 'fa fa-list-alt', label: 'All Templates', color: 'var(--g-primary)', bg: 'rgba(129,140,248,.12)' },
    { route: '/compliance/templates/new', icon: 'fa fa-plus', label: 'New Template', color: 'var(--g-success)', bg: 'var(--g-success-bg)' },
    { route: '/identity/users', icon: 'fa fa-users', label: 'User Management', color: 'var(--g-cyan)', bg: 'var(--g-info-bg)' },
    { route: '/identity/roles', icon: 'fa fa-user-secret', label: 'Role Management', color: 'var(--g-warning)', bg: 'var(--g-warning-bg)' },
    { route: '/setting-management', icon: 'fa fa-cog', label: 'Settings', color: 'var(--g-text-muted)', bg: 'var(--g-surface-3)' },
  ];
}

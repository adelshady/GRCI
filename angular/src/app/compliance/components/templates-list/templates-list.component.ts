import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { FormBuilder, FormGroup } from '@angular/forms';
import { ConfirmationService, ToasterService } from '@abp/ng.theme.shared';
import { Confirmation } from '@abp/ng.theme.shared';
import {
  ChecklistTemplateDto,
  ChecklistTemplateStatus,
  ListTemplatesInput,
  STATUS_BADGE_CLASS,
  STATUS_LABELS,
} from '../../models/compliance.models';
import { ComplianceTemplatesService } from '../../services/compliance-templates.service';

@Component({
  selector: 'app-templates-list',
  templateUrl: './templates-list.component.html',
  styleUrls: ['./templates-list.component.scss'],
})
export class TemplatesListComponent implements OnInit {
  templates: ChecklistTemplateDto[] = [];
  totalCount = 0;
  loading = false;

  pageSize = 10;
  currentPage = 1;

  filterForm: FormGroup;

  statusOptions = [
    { value: null, label: '::Compliance:Status:All' },
    { value: ChecklistTemplateStatus.Draft, label: '::Compliance:Status:Draft' },
    { value: ChecklistTemplateStatus.PendingApproval, label: '::Compliance:Status:PendingApproval' },
    { value: ChecklistTemplateStatus.Approved, label: '::Compliance:Status:Approved' },
    { value: ChecklistTemplateStatus.Returned, label: '::Compliance:Status:Returned' },
    { value: ChecklistTemplateStatus.Archived, label: '::Compliance:Status:Archived' },
  ];

  readonly STATUS_BADGE_CLASS = STATUS_BADGE_CLASS;
  readonly STATUS_LABELS = STATUS_LABELS;
  readonly ChecklistTemplateStatus = ChecklistTemplateStatus;

  constructor(
    private templatesService: ComplianceTemplatesService,
    private router: Router,
    private fb: FormBuilder,
    private confirmation: ConfirmationService,
    private toastr: ToasterService
  ) {
    this.filterForm = this.fb.group({
      search: [''],
      status: [null],
    });
  }

  ngOnInit(): void {
    this.loadTemplates();
  }

  loadTemplates(): void {
    this.loading = true;
    const { search, status } = this.filterForm.value;
    const input: ListTemplatesInput = {
      search: search || undefined,
      status: status !== null && status !== undefined ? status : undefined,
      skipCount: (this.currentPage - 1) * this.pageSize,
      maxResultCount: this.pageSize,
    };

    this.templatesService.getList(input).subscribe({
      next: result => {
        this.templates = result.items;
        this.totalCount = result.totalCount;
        this.loading = false;
      },
      error: () => {
        this.loading = false;
        this.toastr.error('::Compliance:Error:LoadFailed', '::Error');
      },
    });
  }

  onFilter(): void {
    this.currentPage = 1;
    this.loadTemplates();
  }

  onPageChange(page: number): void {
    this.currentPage = page;
    this.loadTemplates();
  }

  canEdit(template: ChecklistTemplateDto): boolean {
    return (
      template.status === ChecklistTemplateStatus.Draft ||
      template.status === ChecklistTemplateStatus.Returned
    );
  }

  viewTemplate(template: ChecklistTemplateDto): void {
    this.router.navigate(['/compliance/templates', template.id]);
  }

  editTemplate(template: ChecklistTemplateDto): void {
    this.router.navigate(['/compliance/templates', template.id, 'edit']);
  }

  deleteTemplate(template: ChecklistTemplateDto): void {
    this.confirmation
      .warn('::Compliance:Template:DeleteConfirmMessage', '::Compliance:Template:DeleteConfirmTitle', {
        messageLocalizationParams: [template.nameEn],
      })
      .subscribe(status => {
        if (status === Confirmation.Status.confirm) {
          this.templatesService.delete(template.id).subscribe({
            next: () => {
              this.toastr.success('::Compliance:Template:DeletedSuccessfully', '::Success');
              this.loadTemplates();
            },
            error: (err: any) => {
              const msg = err?.error?.error?.message;
              this.toastr.error(msg || '::Compliance:Error:DeleteFailed', '::Error');
            },
          });
        }
      });
  }

  formatStatus(status: ChecklistTemplateStatus): string {
    return STATUS_LABELS[status] || status.toString();
  }

  getBadgeClass(status: ChecklistTemplateStatus): string {
    return STATUS_BADGE_CLASS[status] || 'bg-secondary';
  }
}

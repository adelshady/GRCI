import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { ConfirmationService, ToasterService } from '@abp/ng.theme.shared';
import { Confirmation } from '@abp/ng.theme.shared';
import {
  ChecklistTemplateDto,
  ChecklistTemplateStatus,
  ChecklistItemDto,
  WorkflowActionHistoryDto,
  STATUS_BADGE_CLASS,
  STATUS_LABELS,
} from '../../models/compliance.models';
import { ComplianceTemplatesService } from '../../services/compliance-templates.service';
import { ComplianceItemsService } from '../../services/compliance-items.service';
import { ComplianceWorkflowService } from '../../services/compliance-workflow.service';
import { ItemEditModalComponent } from '../item-edit-modal/item-edit-modal.component';
import { AttachmentsModalComponent } from '../attachments-modal/attachments-modal.component';

@Component({
  selector: 'app-template-detail',
  templateUrl: './template-detail.component.html',
  styleUrls: ['./template-detail.component.scss'],
})
export class TemplateDetailComponent implements OnInit {
  template?: ChecklistTemplateDto;
  history: WorkflowActionHistoryDto[] = [];
  loading = false;
  actionLoading = false;
  returnComment = '';
  showReturnModal = false;
  activeTab: 'items' | 'history' = 'items';

  readonly STATUS_BADGE_CLASS = STATUS_BADGE_CLASS;
  readonly STATUS_LABELS = STATUS_LABELS;
  readonly ChecklistTemplateStatus = ChecklistTemplateStatus;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private templatesService: ComplianceTemplatesService,
    private itemsService: ComplianceItemsService,
    private workflowService: ComplianceWorkflowService,
    private modalService: NgbModal,
    private confirmation: ConfirmationService,
    private toastr: ToasterService
  ) {}

  ngOnInit(): void {
    const id = this.route.snapshot.paramMap.get('id')!;
    this.loadTemplate(id);
  }

  loadTemplate(id?: string): void {
    const templateId = id || this.template!.id;
    this.loading = true;
    this.templatesService.get(templateId).subscribe({
      next: template => {
        this.template = template;
        this.loadHistory();
        this.loading = false;
      },
      error: () => {
        this.loading = false;
        this.toastr.error('::Compliance:Error:LoadFailed', '::Error');
      },
    });
  }

  loadHistory(): void {
    if (!this.template) return;
    this.workflowService.getHistory(this.template.id).subscribe({
      next: history => {
        this.history = history.sort(
          (a, b) => new Date(b.performedAt).getTime() - new Date(a.performedAt).getTime()
        );
      },
    });
  }

  get activeItems(): ChecklistItemDto[] {
    return (this.template?.items || []).filter(i => i.isActive).sort((a, b) => a.sortOrder - b.sortOrder);
  }

  get canEdit(): boolean {
    return (
      this.template?.status === ChecklistTemplateStatus.Draft ||
      this.template?.status === ChecklistTemplateStatus.Returned
    );
  }

  getBadgeClass(status: ChecklistTemplateStatus): string {
    return STATUS_BADGE_CLASS[status] || 'bg-secondary';
  }

  getStatusLabel(status: ChecklistTemplateStatus): string {
    return STATUS_LABELS[status] || status.toString();
  }

  // ── Workflow Actions ──────────────────────────────────────────────────────

  submit(): void {
    this.confirmation
      .warn('::Compliance:Workflow:SubmitConfirm', '::Compliance:Workflow:SubmitTitle')
      .subscribe(status => {
        if (status === Confirmation.Status.confirm) {
          this.doWorkflowAction(() => this.workflowService.submit(this.template!.id), '::Compliance:Workflow:Submitted');
        }
      });
  }

  approve(): void {
    this.confirmation
      .warn('::Compliance:Workflow:ApproveConfirm', '::Compliance:Workflow:ApproveTitle')
      .subscribe(status => {
        if (status === Confirmation.Status.confirm) {
          this.doWorkflowAction(() => this.workflowService.approve(this.template!.id), '::Compliance:Workflow:Approved');
        }
      });
  }

  openReturnModal(returnModal: any): void {
    this.returnComment = '';
    this.modalService.open(returnModal, { centered: true });
  }

  confirmReturn(modal: any): void {
    if (!this.returnComment?.trim()) {
      this.toastr.warn('::Compliance:Workflow:CommentRequired', '::Warning');
      return;
    }
    modal.close();
    this.doWorkflowAction(
      () => this.workflowService.return(this.template!.id, this.returnComment),
      '::Compliance:Workflow:Returned'
    );
  }

  resubmit(): void {
    this.confirmation
      .warn('::Compliance:Workflow:ResubmitConfirm', '::Compliance:Workflow:ResubmitTitle')
      .subscribe(status => {
        if (status === Confirmation.Status.confirm) {
          this.doWorkflowAction(() => this.workflowService.resubmit(this.template!.id), '::Compliance:Workflow:Resubmitted');
        }
      });
  }

  archive(): void {
    this.confirmation
      .warn('::Compliance:Workflow:ArchiveConfirm', '::Compliance:Workflow:ArchiveTitle')
      .subscribe(status => {
        if (status === Confirmation.Status.confirm) {
          this.doWorkflowAction(() => this.workflowService.archive(this.template!.id), '::Compliance:Workflow:Archived');
        }
      });
  }

  private doWorkflowAction(action: () => any, successMsg: string): void {
    this.actionLoading = true;
    action().subscribe({
      next: () => {
        this.actionLoading = false;
        this.toastr.success(successMsg, '::Success');
        this.loadTemplate();
      },
      error: (err: any) => {
        this.actionLoading = false;
        const msg = err?.error?.error?.message;
        this.toastr.error(msg || '::Compliance:Error:ActionFailed', '::Error');
      },
    });
  }

  // ── Item Actions ──────────────────────────────────────────────────────────

  openAddItem(): void {
    const ref = this.modalService.open(ItemEditModalComponent, { size: 'lg', centered: true });
    ref.componentInstance.templateId = this.template!.id;
    ref.componentInstance.item = null;
    ref.result
      .then(result => { if (result === 'saved') this.loadTemplate(); })
      .catch(() => {});
  }

  openEditItem(item: ChecklistItemDto): void {
    const ref = this.modalService.open(ItemEditModalComponent, { size: 'lg', centered: true });
    ref.componentInstance.templateId = this.template!.id;
    ref.componentInstance.item = item;
    ref.result
      .then(result => { if (result === 'saved') this.loadTemplate(); })
      .catch(() => {});
  }

  deleteItem(item: ChecklistItemDto): void {
    this.confirmation
      .warn('::Compliance:Item:DeleteConfirm', '::Compliance:Item:DeleteTitle')
      .subscribe(status => {
        if (status === Confirmation.Status.confirm) {
          this.itemsService.delete(item.id).subscribe({
            next: () => {
              this.toastr.success('::Compliance:Item:DeletedSuccessfully', '::Success');
              this.loadTemplate();
            },
            error: (err: any) => this.toastr.error(err?.error?.error?.message || '::Compliance:Error:DeleteFailed', '::Error'),
          });
        }
      });
  }

  openAttachments(item: ChecklistItemDto): void {
    const ref = this.modalService.open(AttachmentsModalComponent, { size: 'lg', centered: true });
    ref.componentInstance.item = item;
    ref.result.catch(() => {});
  }

  // ── Helpers ───────────────────────────────────────────────────────────────

  getCriticalityLabel(criticality: number): string {
    const labels: Record<number, string> = { 1: '::Compliance:Criticality:Minor', 2: '::Compliance:Criticality:Medium', 3: '::Compliance:Criticality:Critical' };
    return labels[criticality] || criticality.toString();
  }

  getCriticalityClass(criticality: number): string {
    const classes: Record<number, string> = { 1: 'bg-success', 2: 'bg-warning text-dark', 3: 'bg-danger' };
    return classes[criticality] || 'bg-secondary';
  }

  getActionIcon(action: string): string {
    const icons: Record<string, string> = {
      Submit: 'fa fa-paper-plane text-warning',
      Approve: 'fa fa-check-circle text-success',
      Return: 'fa fa-reply text-danger',
      Resubmit: 'fa fa-redo text-info',
      Archive: 'fa fa-archive text-secondary',
    };
    return icons[action] || 'fa fa-circle text-muted';
  }

  getActionLabel(action: string): string {
    const labels: Record<string, string> = {
      Submit: '::Compliance:Workflow:Submit',
      Approve: '::Compliance:Workflow:Approve',
      Return: '::Compliance:Workflow:Return',
      Resubmit: '::Compliance:Workflow:Resubmit',
      Archive: '::Compliance:Workflow:Archive',
    };
    return labels[action] || action;
  }

  getHistoryStatusLabel(status: string): string {
    const labels: Record<string, string> = {
      Draft: '::Compliance:Status:Draft',
      PendingApproval: '::Compliance:Status:PendingApproval',
      Approved: '::Compliance:Status:Approved',
      Returned: '::Compliance:Status:Returned',
      Archived: '::Compliance:Status:Archived',
    };
    return labels[status] || status;
  }

  goBack(): void {
    this.router.navigate(['/compliance/templates']);
  }
}

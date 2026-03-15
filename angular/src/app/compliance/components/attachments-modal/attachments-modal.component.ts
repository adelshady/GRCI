import { Component, Input, OnInit } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { ConfirmationService, ToasterService } from '@abp/ng.theme.shared';
import { Confirmation } from '@abp/ng.theme.shared';
import { ComplianceAttachmentsService } from '../../services/compliance-attachments.service';
import { AttachmentDto, ChecklistItemDto } from '../../models/compliance.models';

@Component({
  selector: 'app-attachments-modal',
  templateUrl: './attachments-modal.component.html',
  styleUrls: ['./attachments-modal.component.scss'],
})
export class AttachmentsModalComponent implements OnInit {
  @Input() item!: ChecklistItemDto;

  attachments: AttachmentDto[] = [];
  loading = false;
  uploading = false;
  selectedFile: File | null = null;

  constructor(
    public activeModal: NgbActiveModal,
    private attachmentsService: ComplianceAttachmentsService,
    private confirmation: ConfirmationService,
    private toastr: ToasterService
  ) {}

  ngOnInit(): void {
    this.loadAttachments();
  }

  loadAttachments(): void {
    this.loading = true;
    this.attachmentsService.getAttachments(this.item.id).subscribe({
      next: attachments => {
        this.attachments = attachments;
        this.loading = false;
      },
      error: () => {
        this.loading = false;
        this.toastr.error('::Compliance:Error:LoadFailed', '::Error');
      },
    });
  }

  onFileSelected(event: Event): void {
    const input = event.target as HTMLInputElement;
    this.selectedFile = input.files?.[0] || null;
  }

  upload(): void {
    if (!this.selectedFile) return;

    this.uploading = true;
    this.attachmentsService.upload(this.item.id, this.selectedFile).subscribe({
      next: () => {
        this.uploading = false;
        this.selectedFile = null;
        // Reset file input
        const fileInput = document.getElementById('fileInput') as HTMLInputElement;
        if (fileInput) fileInput.value = '';
        this.toastr.success('::Compliance:Attachment:UploadedSuccessfully', '::Success');
        this.loadAttachments();
      },
      error: err => {
        this.uploading = false;
        const message = err?.error?.error?.message || '::Compliance:Error:UploadFailed';
        this.toastr.error(message, '::Error');
      },
    });
  }

  download(attachment: AttachmentDto): void {
    this.attachmentsService.download(attachment.fileId).subscribe({
      next: blob => {
        const url = window.URL.createObjectURL(blob);
        const a = document.createElement('a');
        a.href = url;
        a.download = attachment.fileName;
        document.body.appendChild(a);
        a.click();
        document.body.removeChild(a);
        window.URL.revokeObjectURL(url);
      },
      error: () => this.toastr.error('::Compliance:Error:DownloadFailed', '::Error'),
    });
  }

  deleteAttachment(attachment: AttachmentDto): void {
    this.confirmation
      .warn('::Compliance:Attachment:DeleteConfirm', '::Compliance:Attachment:DeleteTitle')
      .subscribe(status => {
        if (status === Confirmation.Status.confirm) {
          this.attachmentsService.delete(attachment.fileId).subscribe({
            next: () => {
              this.toastr.success('::Compliance:Attachment:DeletedSuccessfully', '::Success');
              this.loadAttachments();
            },
            error: () => this.toastr.error('::Compliance:Error:DeleteFailed', '::Error'),
          });
        }
      });
  }

  formatFileSize(bytes: number): string {
    if (bytes < 1024) return `${bytes} B`;
    if (bytes < 1024 * 1024) return `${(bytes / 1024).toFixed(1)} KB`;
    return `${(bytes / (1024 * 1024)).toFixed(1)} MB`;
  }

  getFileIcon(contentType: string): string {
    if (contentType.includes('pdf')) return 'fa fa-file-pdf text-danger';
    if (contentType.includes('word') || contentType.includes('docx')) return 'fa fa-file-word text-primary';
    if (contentType.includes('image')) return 'fa fa-file-image text-success';
    return 'fa fa-file text-secondary';
  }

  dismiss(): void {
    this.activeModal.dismiss();
  }
}

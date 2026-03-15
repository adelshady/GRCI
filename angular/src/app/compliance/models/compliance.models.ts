export enum ChecklistTemplateStatus {
  Draft = 0,
  PendingApproval = 1,
  Approved = 2,
  Returned = 3,
  Archived = 4,
}

export enum ItemCriticality {
  Minor = 1,
  Medium = 2,
  Critical = 3,
}

export enum NotesRequirement {
  Never = 0,
  WhenNonCompliant = 1,
  Always = 2,
}

export interface ChecklistTemplateDto {
  id: string;
  code: string;
  nameEn: string;
  nameAr?: string;
  descriptionEn?: string;
  descriptionAr?: string;
  regulatoryAgencyId: string;
  status: ChecklistTemplateStatus;
  version: number;
  items: ChecklistItemDto[];
  workflowHistory?: WorkflowActionHistoryDto[];
  creationTime: string;
  lastModificationTime?: string;
}

export interface ChecklistItemDto {
  id: string;
  templateId: string;
  sectionTitle?: string;
  requirementTextEn: string;
  requirementTextAr?: string;
  criticality: ItemCriticality;
  weight: number;
  isMandatory: boolean;
  notesRequiredWhen: NotesRequirement;
  sortOrder: number;
  isActive: boolean;
}

export interface WorkflowActionHistoryDto {
  id: string;
  entityType: string;
  entityId: string;
  action: string;
  fromStatus: string;
  toStatus: string;
  performedByUserId?: string;
  performedByUserName?: string;
  performedAt: string;
  comment?: string;
}

export interface AttachmentDto {
  fileId: string;
  linkId: string;
  entityType: string;
  entityId: string;
  fileName: string;
  contentType: string;
  size: number;
  uploadedByUserId?: string;
  uploadedAt: string;
}

export interface RegulatoryAgencyDto {
  id: string;
  nameEn: string;
  nameAr?: string;
  code?: string;
  isActive: boolean;
}

export interface PagedResult<T> {
  items: T[];
  totalCount: number;
}

export interface CreateUpdateTemplateInput {
  nameEn: string;
  nameAr?: string;
  descriptionEn?: string;
  descriptionAr?: string;
  regulatoryAgencyId: string;
}

export interface CreateUpdateItemInput {
  sectionTitle?: string;
  requirementTextEn: string;
  requirementTextAr?: string;
  criticality: ItemCriticality;
  isMandatory: boolean;
  notesRequiredWhen: NotesRequirement;
  sortOrder: number;
}

export interface ListTemplatesInput {
  search?: string;
  status?: ChecklistTemplateStatus | null;
  regulatoryAgencyId?: string;
  skipCount?: number;
  maxResultCount?: number;
}

export const STATUS_LABELS: Record<ChecklistTemplateStatus, string> = {
  [ChecklistTemplateStatus.Draft]: '::Compliance:Status:Draft',
  [ChecklistTemplateStatus.PendingApproval]: '::Compliance:Status:PendingApproval',
  [ChecklistTemplateStatus.Approved]: '::Compliance:Status:Approved',
  [ChecklistTemplateStatus.Returned]: '::Compliance:Status:Returned',
  [ChecklistTemplateStatus.Archived]: '::Compliance:Status:Archived',
};

export const STATUS_BADGE_CLASS: Record<ChecklistTemplateStatus, string> = {
  [ChecklistTemplateStatus.Draft]: 'bg-secondary',
  [ChecklistTemplateStatus.PendingApproval]: 'bg-warning text-dark',
  [ChecklistTemplateStatus.Approved]: 'bg-success',
  [ChecklistTemplateStatus.Returned]: 'bg-danger',
  [ChecklistTemplateStatus.Archived]: 'bg-dark',
};

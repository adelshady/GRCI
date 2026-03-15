import { Component, Input, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { ToasterService } from '@abp/ng.theme.shared';
import { ComplianceItemsService } from '../../services/compliance-items.service';
import { ChecklistItemDto, ItemCriticality, NotesRequirement } from '../../models/compliance.models';

@Component({
  selector: 'app-item-edit-modal',
  templateUrl: './item-edit-modal.component.html',
  styleUrls: ['./item-edit-modal.component.scss'],
})
export class ItemEditModalComponent implements OnInit {
  @Input() templateId!: string;
  @Input() item: ChecklistItemDto | null = null;

  form: FormGroup;
  saving = false;
  derivedWeight = 1;

  readonly ItemCriticality = ItemCriticality;
  readonly NotesRequirement = NotesRequirement;

  criticalityOptions = [
    { value: ItemCriticality.Minor, label: '::Compliance:Criticality:Minor', weight: 1 },
    { value: ItemCriticality.Medium, label: '::Compliance:Criticality:Medium', weight: 2 },
    { value: ItemCriticality.Critical, label: '::Compliance:Criticality:Critical', weight: 3 },
  ];

  notesOptions = [
    { value: NotesRequirement.Never, label: '::Compliance:Notes:Never' },
    { value: NotesRequirement.WhenNonCompliant, label: '::Compliance:Notes:WhenNonCompliant' },
    { value: NotesRequirement.Always, label: '::Compliance:Notes:Always' },
  ];

  constructor(
    public activeModal: NgbActiveModal,
    private fb: FormBuilder,
    private itemsService: ComplianceItemsService,
    private toastr: ToasterService
  ) {
    this.form = this.fb.group({
      sectionTitle: ['', [Validators.maxLength(256)]],
      requirementTextEn: ['', [Validators.required, Validators.maxLength(4000)]],
      requirementTextAr: ['', [Validators.maxLength(4000)]],
      criticality: [ItemCriticality.Minor, [Validators.required]],
      isMandatory: [false],
      notesRequiredWhen: [NotesRequirement.Never],
      sortOrder: [0, [Validators.min(0)]],
    });
  }

  ngOnInit(): void {
    if (this.item) {
      this.form.patchValue({
        sectionTitle: this.item.sectionTitle || '',
        requirementTextEn: this.item.requirementTextEn,
        requirementTextAr: this.item.requirementTextAr || '',
        criticality: this.item.criticality,
        isMandatory: this.item.isMandatory,
        notesRequiredWhen: this.item.notesRequiredWhen,
        sortOrder: this.item.sortOrder,
      });
      this.updateWeight(this.item.criticality);
    }

    this.form.get('criticality')!.valueChanges.subscribe(val => this.updateWeight(val));
  }

  updateWeight(criticality: ItemCriticality): void {
    const opt = this.criticalityOptions.find(o => o.value === +criticality);
    this.derivedWeight = opt?.weight ?? 1;
  }

  save(): void {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    this.saving = true;
    const v = this.form.value;
    const input = {
      sectionTitle: v.sectionTitle || undefined,
      requirementTextEn: v.requirementTextEn,
      requirementTextAr: v.requirementTextAr || undefined,
      criticality: +v.criticality,
      isMandatory: v.isMandatory,
      notesRequiredWhen: +v.notesRequiredWhen,
      sortOrder: v.sortOrder,
    };

    const request = this.item
      ? this.itemsService.update(this.item.id, input)
      : this.itemsService.create(this.templateId, input);

    request.subscribe({
      next: () => {
        this.saving = false;
        this.toastr.success('::Compliance:Item:SavedSuccessfully', '::Success');
        this.activeModal.close('saved');
      },
      error: (err: any) => {
        this.saving = false;
        const msg = err?.error?.error?.message;
        this.toastr.error(msg || '::Compliance:Error:SaveFailed', '::Error');
      },
    });
  }

  dismiss(): void {
    this.activeModal.dismiss();
  }
}

import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { ToasterService } from '@abp/ng.theme.shared';
import { LocalizationService } from '@abp/ng.core';
import { ComplianceTemplatesService } from '../../services/compliance-templates.service';
import { ComplianceAgenciesService } from '../../services/compliance-agencies.service';
import { ChecklistTemplateDto, RegulatoryAgencyDto } from '../../models/compliance.models';

@Component({
  selector: 'app-template-create-edit',
  templateUrl: './template-create-edit.component.html',
  styleUrls: ['./template-create-edit.component.scss'],
})
export class TemplateCreateEditComponent implements OnInit {
  form: FormGroup;
  isEditMode = false;
  templateId?: string;
  loading = false;
  saving = false;
  agencies: RegulatoryAgencyDto[] = [];

  constructor(
    private fb: FormBuilder,
    private route: ActivatedRoute,
    private router: Router,
    private templatesService: ComplianceTemplatesService,
    private agenciesService: ComplianceAgenciesService,
    private toastr: ToasterService,
    private localization: LocalizationService
  ) {
    this.form = this.fb.group({
      nameEn: ['', [Validators.required, Validators.maxLength(256)]],
      nameAr: ['', [Validators.maxLength(256)]],
      descriptionEn: ['', [Validators.maxLength(2000)]],
      descriptionAr: ['', [Validators.maxLength(2000)]],
      regulatoryAgencyId: ['', [Validators.required]],
    });
  }

  ngOnInit(): void {
    this.agenciesService.getList().subscribe({
      next: agencies => (this.agencies = agencies),
      error: () => this.toastr.error('::Compliance:Error:LoadFailed', '::Error'),
    });

    this.templateId = this.route.snapshot.paramMap.get('id') || undefined;
    this.isEditMode = !!this.templateId;

    if (this.isEditMode) {
      this.loading = true;
      this.templatesService.get(this.templateId!).subscribe({
        next: template => {
          this.form.patchValue({
            nameEn: template.nameEn,
            nameAr: template.nameAr || '',
            descriptionEn: template.descriptionEn || '',
            descriptionAr: template.descriptionAr || '',
            regulatoryAgencyId: template.regulatoryAgencyId,
          });
          this.loading = false;
        },
        error: () => {
          this.loading = false;
          this.toastr.error('::Compliance:Error:LoadFailed', '::Error');
        },
      });
    }
  }

  save(): void {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    this.saving = true;
    const value = this.form.value;
    const input = {
      nameEn: value.nameEn,
      nameAr: value.nameAr || undefined,
      descriptionEn: value.descriptionEn || undefined,
      descriptionAr: value.descriptionAr || undefined,
      regulatoryAgencyId: value.regulatoryAgencyId,
    };

    const request = this.isEditMode
      ? this.templatesService.update(this.templateId!, input)
      : this.templatesService.create(input);

    request.subscribe({
      next: (result: ChecklistTemplateDto) => {
        this.saving = false;
        this.toastr.success('::Compliance:Template:SavedSuccessfully', '::Success');
        this.router.navigate(['/compliance/templates', result.id]);
      },
      error: (err: any) => {
        this.saving = false;
        const msg = err?.error?.error?.message;
        this.toastr.error(msg || '::Compliance:Error:SaveFailed', '::Error');
      },
    });
  }

  getAgencyDisplayName(agency: RegulatoryAgencyDto): string {
    const isArabic = this.localization.currentLang?.startsWith('ar');
    return (isArabic && agency.nameAr) ? agency.nameAr : agency.nameEn;
  }

  cancel(): void {
    if (this.isEditMode) {
      this.router.navigate(['/compliance/templates', this.templateId]);
    } else {
      this.router.navigate(['/compliance/templates']);
    }
  }
}

import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { NgbModalModule, NgbPaginationModule } from '@ng-bootstrap/ng-bootstrap';
import { CoreModule } from '@abp/ng.core';
import { ThemeSharedModule } from '@abp/ng.theme.shared';

import { ComplianceRoutingModule } from './compliance-routing.module';
import { TemplatesListComponent } from './components/templates-list/templates-list.component';
import { TemplateCreateEditComponent } from './components/template-create-edit/template-create-edit.component';
import { TemplateDetailComponent } from './components/template-detail/template-detail.component';
import { ItemEditModalComponent } from './components/item-edit-modal/item-edit-modal.component';
import { AttachmentsModalComponent } from './components/attachments-modal/attachments-modal.component';

@NgModule({
  declarations: [
    TemplatesListComponent,
    TemplateCreateEditComponent,
    TemplateDetailComponent,
    ItemEditModalComponent,
    AttachmentsModalComponent,
  ],
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    RouterModule,
    NgbModalModule,
    NgbPaginationModule,
    CoreModule,
    ThemeSharedModule,
    ComplianceRoutingModule,
  ],
})
export class ComplianceModule {}

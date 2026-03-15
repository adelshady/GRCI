import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { TemplatesListComponent } from './components/templates-list/templates-list.component';
import { TemplateCreateEditComponent } from './components/template-create-edit/template-create-edit.component';
import { TemplateDetailComponent } from './components/template-detail/template-detail.component';

const routes: Routes = [
  { path: '', component: TemplatesListComponent },
  { path: 'new', component: TemplateCreateEditComponent },
  { path: ':id', component: TemplateDetailComponent },
  { path: ':id/edit', component: TemplateCreateEditComponent },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class ComplianceRoutingModule {}

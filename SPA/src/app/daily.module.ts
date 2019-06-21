import { EmpAddModalComponent } from './daily/daily-file-details/emp-add-modal/emp-add-modal.component';
import { SumPipe } from './_pipe/sum.pipe';
import { FileSheetsModalComponent } from './daily/daily-file-details/file-sheets-modal/file-sheets-modal.component';
import { PreventFileUnsavedChangedService } from './_guard/PreventFileUnsavedChanged.service';
import { DailyFileDetailsComponent } from './daily/daily-file-details/daily-file-details.component';

import { FiletypeModalComponent } from './daily/daily-file/filetype-modal/filetype-modal.component';
import { HttpClientModule } from '@angular/common/http';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterModule, Routes } from '@angular/router';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
// component
import { DailyFileComponent } from './daily/daily-file/daily-file.component';

import { FileUploadModule } from 'ng2-file-upload';
import { ComponentsModule } from './components.module';
import { AuthGuardService } from './_guard/AuthGuard.service';
import { SearchPipe } from './_pipe/search.pipe';

const dailyRoutes: Routes = [
  {
    path: 'files',
    component: DailyFileComponent,
    canActivate: [AuthGuardService],
    canDeactivate: [PreventFileUnsavedChangedService]
  },
  { path: 'files/:id', component: DailyFileDetailsComponent }
];
@NgModule({
  declarations: [
    DailyFileComponent,
    FiletypeModalComponent,
    DailyFileDetailsComponent,
    FileSheetsModalComponent,
    EmpAddModalComponent,
    SumPipe,
   SearchPipe
  ],
  imports: [
    CommonModule,
    HttpClientModule,
    FormsModule,
    // SharedModule,
    FileUploadModule,
    RouterModule.forChild(dailyRoutes),
    ReactiveFormsModule,
    ComponentsModule
  ],
  entryComponents: [
    FiletypeModalComponent,
    FileSheetsModalComponent,
    EmpAddModalComponent
  ],

  providers: [PreventFileUnsavedChangedService],
  exports: [RouterModule]
})
export class DailyModule {}

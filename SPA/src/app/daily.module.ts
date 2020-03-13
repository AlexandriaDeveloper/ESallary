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
import { DailyFileListComponent } from './daily/daily-file-list/daily-file-list.component';
import { FileResolver } from './_resolver/file-list.resolver';
import { EditFileComponent } from './daily/daily-file-list/edit-file/edit-file.component';

import { SerachModelComponent } from './daily/daily-file-list/serach-model/serach-model.component';
import { DailyListComponent } from './daily/daily-list/daily-list.component';
import { DailyResolver } from './_resolver/daily-list.resolver';


const dailyRoutes: Routes = [
  {
    path: 'files/new',
    component: DailyFileComponent,
    canActivate: [AuthGuardService],
    canDeactivate: [PreventFileUnsavedChangedService]
  },
  { path: 'files/:id', component: DailyFileDetailsComponent },
  { path: 'files/edit-file/:id', component: EditFileComponent },

  {
    path: 'files',
    component: DailyFileListComponent,
    resolve: { emps: FileResolver }
  },
  {
    path: '',
    component: DailyListComponent,
    resolve: { emps: DailyResolver }
  }
];
@NgModule({
  declarations: [
    DailyFileComponent,
    FiletypeModalComponent,
    DailyFileDetailsComponent,
    FileSheetsModalComponent,
    EmpAddModalComponent,
    SumPipe,
    SearchPipe,
    DailyFileListComponent,

    EditFileComponent,

    SerachModelComponent,

    DailyListComponent
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
    EmpAddModalComponent,
    SerachModelComponent
  ],

  providers: [PreventFileUnsavedChangedService, FileResolver, DailyResolver],
  exports: [RouterModule]
})
export class DailyModule {}

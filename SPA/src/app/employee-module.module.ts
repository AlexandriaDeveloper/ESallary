import { TestChildComponent } from './employees/test-parent/test-child/test-child.component';
import { TestParentComponent } from './employees/test-parent/test-parent.component';
import { ComponentsModule } from './components.module';
import { RestoreDialogComponent } from './_components/restore-dialog/restore-dialog.component';
import { EmployeeRestoreResolver } from './_resolver/employee-restore.resolver';
import { EmployeeRestoreComponent } from './employees/employees-list/employee-details/employee-restore/employee-restore.component';
import { DeleteDialogComponent } from './_components/delete-dialog/delete-dialog.component';
import { EmployeeListRowComponent } from './employees/employees-list/employee-list-row/employee-list-row.component';
import { SharedModule } from './shared.module';

// tslint:disable-next-line:import-spacing

import { EmployeeSharedService } from './_services/employee-shared.service';

import { PreventUnsavedChanged } from './_guard/PreventUnsavedChanged';

import { EmployeePageComponent } from './employees/employee-page/employee-page.component';
import { HttpClientModule } from '@angular/common/http';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterModule, Routes } from '@angular/router';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { EmployeesListComponent } from './employees/employees-list/employees-list.component';

import { AuthGuardService } from './_guard/AuthGuard.service';
import { EmployeeDetailsResolver } from './_resolver/employee-details.resolver';
import { EmpsListResolver } from './_resolver/emps-list.resolver';
import { EmployeeAddComponent } from './employees/employee-add/employee-add.component';
import { PaginationModule } from 'ngx-bootstrap/pagination';
import { NgxSelectModule, INgxSelectOptions } from 'ngx-select-ex';
import { EmployeeDatabaseComponent } from './employees/employee-database/employee-database.component';

import { FileUploadModule } from 'ng2-file-upload';
import { UploadComponent } from './_components/upload/upload.component';

// tslint:disable-next-line:max-line-length
import { EmployeeDetailsOrderComponent } from './employees/employees-list/employee-details/employee-details-order/employee-details-order.component';
import { EmployeeDetailsComponent } from './employees/employees-list/employee-details/employee-details.component';
// tslint:disable-next-line:import-spacing
// tslint:disable-next-line:max-line-length
import { EmployeeDetailsDataComponent } from './employees/employees-list/employee-details/employee-details-data/employee-details-data.component';
// tslint:disable-next-line:max-line-length
import { EmployeeDetailsAddPostComponent } from './employees/employees-list/employee-details/employee-details-add-post/employee-details-add-post.component';
// tslint:disable-next-line:max-line-length
import { EmployeeDetailsAddPostModalComponent } from './employees/employees-list/employee-details/employee-details-add-post/employee-details-add-post-modal/employee-details-add-post-modal.component';

// tslint:disable-next-line:max-line-length
// tslint:disable-next-line:max-line-length
import { EmployeeDetalisAddOrderBankModalComponent } from './employees/employees-list/employee-details/employee-details-order/employee-details-add-order-modal/employee-detalis-add-order-bank-model/employee-detalis-add-order-bank-model.component';
// tslint:disable-next-line:max-line-length
// tslint:disable-next-line:max-line-length
// tslint:disable-next-line:import-spacing
import {
  EmployeeDetailsAddOrderBranchModalComponent
  // tslint:disable-next-line:max-line-length
} from './employees/employees-list/employee-details/employee-details-order/employee-details-add-order-modal/employee-details-add-order-branch/employee-details-add-order-branch.component';
// tslint:disable-next-line:max-line-length
// tslint:disable-next-line:import-spacing
import {
  EmployeeDetailsAddOrderModalComponent
  // tslint:disable-next-line:max-line-length
} from './employees/employees-list/employee-details/employee-details-order/employee-details-add-order-modal/employee-details-add-order-modal.component';
// tslint:disable-next-line:import-spacing
import { EmployeeRestoreRowComponent } from './employees/employees-list/employee-details/employee-restore/employee-restore-row/employee-restore-row.component';
// tslint:disable-next-line:import-spacing
import { EmployeeFinincialDataComponent } from './employees/employees-list/employee-details/employee-finincial-data/employee-finincial-data.component';
import { ClickOutsideDirective } from './_directives/clickOutside.directive';
import { OnlyNumberDirective } from './_directives/onlyNumber.directive';
import { NullDefaultValueDirective } from './_directives/nullDefaultValue.directive';

const routes: Routes = [
  {
    /**?pageNumber=3&pageSize=5 */
    path: '',
    component: EmployeePageComponent
  },
  {
    /**?pageNumber=3&pageSize=5 */
    path: 'list',
    component: EmployeesListComponent,
    data: { titel: 'EFinance ' },
    canActivate: [AuthGuardService],
    resolve: { emps: EmpsListResolver }
  },
  {
    /**?pageNumber=3&pageSize=5 */
    path: 'restore',
    component: EmployeeRestoreComponent,
    data: { titel: 'EFinance ' },
    canActivate: [AuthGuardService],
    resolve: { emps: EmployeeRestoreResolver }
  },
  {
    path: 'upload',
    component: EmployeeDatabaseComponent,
    data: { titel: 'EFinance ', depth: 2 },
    canActivate: [AuthGuardService]
  },
  {
    /**?pageNumber=3&pageSize=5 */
    path: 'details/:nationalId',
    component: EmployeeDetailsComponent,
    data: { titel: 'بيانات الموظف ' },
    canActivate: [AuthGuardService],
    resolve: { emp: EmployeeDetailsResolver },
    canDeactivate: [PreventUnsavedChanged]
  },
  {
    /**?pageNumber=3&pageSize=5 */
    path: 'details/:id/finance',
    component: EmployeeFinincialDataComponent,
    data: { titel: '  بيانات الموظف  الماليه' },
    canActivate: [AuthGuardService]
  },
  {
    path: 'add',
    component: EmployeeAddComponent,
    canActivate: [AuthGuardService]
  },
  {
    path: 'test',
    component: TestParentComponent,
    canActivate: [AuthGuardService]
  }
];
export const CustomSelectOptions: INgxSelectOptions = {
  // Check the interface for more options
  optionValueField: 'id',
  optionTextField: 'name',
  keepSelectedItems: true,
  placeholder: 'أختار '
};
@NgModule({
  declarations: [
    EmployeesListComponent,
    EmployeeListRowComponent,
    EmployeeDetailsComponent,
    EmployeeDetailsDataComponent,
    EmployeeDetailsOrderComponent,
    EmployeeDatabaseComponent,
    EmployeeAddComponent,
    EmployeePageComponent,
    EmployeeDetailsAddPostComponent,
    EmployeeDetailsAddPostComponent,
    EmployeeDetailsAddOrderModalComponent,
    EmployeeDetalisAddOrderBankModalComponent,
    EmployeeDetailsAddOrderBranchModalComponent,
    EmployeeDetailsAddPostModalComponent,
    EmployeeRestoreRowComponent,
    EmployeeRestoreComponent,
    EmployeeFinincialDataComponent,
    UploadComponent,

    RestoreDialogComponent,
    TestParentComponent,
    TestChildComponent
  ],
  imports: [
    CommonModule,
    // SharedModule,
    FormsModule,
    HttpClientModule,
    ReactiveFormsModule,
    NgxSelectModule.forRoot(CustomSelectOptions),
    PaginationModule.forRoot(),
    RouterModule.forChild(routes),
    FileUploadModule,
    ComponentsModule
  ],
  providers: [
    PreventUnsavedChanged,
    EmpsListResolver,
    EmployeeDetailsResolver,
    EmployeeSharedService
  ],
  exports: [RouterModule],
  entryComponents: [
    EmployeeDetailsAddOrderModalComponent,
    EmployeeDetalisAddOrderBankModalComponent,
    EmployeeDetailsAddOrderBranchModalComponent,
    EmployeeDetailsAddPostModalComponent,

    RestoreDialogComponent,
    TestChildComponent
  ]
})
export class EmployeeModuleModule {}

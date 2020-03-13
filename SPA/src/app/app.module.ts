import { SumPipe } from './_pipe/sum.pipe';
import { DepartmentService } from './_services/department.service';

import { BankService } from './_services/bank.service';
import { SharedModule } from './shared.module';

import { CustomToastrService } from './_services/toastr.service';

import { EmployeeService } from './_services/employee.service';

import { AuthService } from './_services/auth.service';
import { LoginComponent } from './users/login/login.component';

import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';

import { AppComponent } from './app.component';


import { TestComponent } from './test/test.component';
import { LayoutComponent } from './layout/layout.component';
import { NavbarComponent } from './layout/navbar/navbar.component';

import { AccountService } from './_services/account.service';
import { HomePageComponent } from './layout/homePage/homePage.component';
import { RouterModule } from '@angular/router';
import { appRoutes } from './app.routes';

import { ComponentsModule } from './components.module';
import { DeleteDialogComponent } from './_components/delete-dialog/delete-dialog.component';
import { DailyService } from './_services/daily.service';

export function tokenGetter() {
  return localStorage.getItem('token');
}
@NgModule({
  declarations: [


    AppComponent,
    TestComponent,
    LayoutComponent,
    NavbarComponent,
    LoginComponent,
    HomePageComponent,
    DeleteDialogComponent,


  ],
  imports: [
    SharedModule,
    BrowserModule,
    BrowserAnimationsModule,
    FormsModule,
    ReactiveFormsModule,
    HttpClientModule,
    ComponentsModule,
    RouterModule.forRoot(appRoutes),
    // JwtModule.forRoot({
    //   config: {
    //     tokenGetter: tokenGetter,
    //     whitelistedDomains: ['localhost:5000'],
    //     blacklistedRoutes: ['localhost:5000/api/auth']
    //   }
    // })
  ],
  providers: [
    AccountService,
    AuthService,
    CustomToastrService,
    CustomToastrService,
    EmployeeService,

    DepartmentService,
    BankService,DailyService

  ],
  bootstrap: [AppComponent],
  exports: [SharedModule],
  entryComponents: [DeleteDialogComponent]
})
export class AppModule {}

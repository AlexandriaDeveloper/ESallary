import { EmployeeDatabaseComponent } from './employees/employee-database/employee-database.component';
import { EmployeeService } from './_services/employee.service';
import { ClickOutsideDirective } from './_directives/clickOutside.directive';
import { AuthService } from './_services/auth.service';
import { LoginComponent } from './users/login/login.component';

import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';

import { AppComponent } from './app.component';

import { JwtModule } from '@auth0/angular-jwt';
import { TestComponent } from './test/test.component';
import { LayoutComponent } from './layout/layout.component';
import { NavbarComponent } from './layout/navbar/navbar.component';
import { CollapseModule } from 'ngx-bootstrap/collapse';
import { CarouselModule } from 'ngx-bootstrap/carousel';
import { AccountService } from './_services/account.service';
import { HomePageComponent } from './layout/homePage/homePage.component';
import { RouterModule } from '@angular/router';
import { appRoutes } from './app.routes';
import { UploadComponent } from './_components/upload/upload.component';
import { FileUploadModule } from 'ng2-file-upload';

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
    UploadComponent,
    EmployeeDatabaseComponent,
    ClickOutsideDirective
  ],
  imports: [
    BrowserModule,
    BrowserAnimationsModule,
    FormsModule,
    HttpClientModule,
    CollapseModule.forRoot(),
    CarouselModule.forRoot(),
    RouterModule.forRoot(appRoutes),
    JwtModule.forRoot({
      config: {
        tokenGetter: tokenGetter,
        whitelistedDomains: ['localhost:5000'],
        blacklistedRoutes: ['localhost:5000/api/auth']
      }
    }),
    FileUploadModule
  ],
  providers: [AccountService, AuthService, EmployeeService],
  bootstrap: [AppComponent]
})
export class AppModule {}

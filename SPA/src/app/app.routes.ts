import { EmployeeDatabaseComponent } from './employees/employee-database/employee-database.component';

import { LoginComponent } from './users/login/login.component';
import { HomePageComponent } from './layout/homePage/homePage.component';
import { Routes } from '@angular/router';
export const appRoutes: Routes = [
  { path: '', component: HomePageComponent },
  { path: 'home', component: HomePageComponent },
  { path: 'login', component: LoginComponent },
  { path: 'emps/upload', component: EmployeeDatabaseComponent },

  // { path: '**', component: PageNotFoundComponent }
];

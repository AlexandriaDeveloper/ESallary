

import { LoginComponent } from './users/login/login.component';
import { HomePageComponent } from './layout/homePage/homePage.component';
import { Routes } from '@angular/router';
export const appRoutes: Routes = [
  {
    path: '',
    component: HomePageComponent,
    data: { titel: 'EFinance ', depth: 2 }
  },
  {
    path: 'home',
    component: HomePageComponent,
    data: { titel: 'EFinance ', depth: 2 }
  },
  {
    path: 'login',
    component: LoginComponent,
    data: { titel: 'EFinance ', depth: 2 }
  },

  {
    path: 'emps',
    loadChildren: './employee-module.module#EmployeeModuleModule'
  },
  {
    path: 'daily',
    loadChildren: './daily.module#DailyModule'
  },
  { path: '**', redirectTo: 'home', data: { titel: 'EFinance ', depth: 1 } }

  // { path: '**', component: PageNotFoundComponent }
];

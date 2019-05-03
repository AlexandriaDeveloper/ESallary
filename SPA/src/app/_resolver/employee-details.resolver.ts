import { catchError, switchMap } from 'rxjs/operators';
import { EmployeeService } from './../_services/employee.service';
import { Injectable } from '@angular/core';

import {
  EmployeeDetailsModel,
  DepartmentModel
} from '../_models/employee.model';
import { Resolve, ActivatedRouteSnapshot } from '@angular/router';
import { Observable, throwError } from 'rxjs';
import { DepartmentService } from '../_services/department.service';

@Injectable({
  providedIn: 'root'
})
export class EmployeeDetailsResolver
  implements Resolve<Observable<EmployeeDetailsModel>> {
  constructor(
    private empService: EmployeeService,
    private departmentService: DepartmentService
  ) {}
  title = '';
  departments: DepartmentModel[];
  resolve(route: ActivatedRouteSnapshot): Observable<EmployeeDetailsModel> {
    const id = route.params['id'];
    this.title = route.data['titel'];

    return this.empService
      .getEmpDetails(id)
      .pipe();
  }
}

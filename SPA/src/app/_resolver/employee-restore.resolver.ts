import { EmpParams } from './../_models/employee.model';
import { PaginatedResult } from '../_models/Pagination';
import { EmployeeService } from '../_services/employee.service';
import { Injectable } from '@angular/core';
import { Resolve, Router, ActivatedRouteSnapshot } from '@angular/router';
import { Observable } from 'rxjs';
import { EmployeeList } from '../_models/employee.model';

@Injectable({
  providedIn: 'root'
})
export class EmployeeRestoreResolver
  implements Resolve<PaginatedResult<EmployeeList[]>> {
  pageNumber = 1;
  pageSize = 30;
  empParams: EmpParams = new EmpParams();
  constructor(private empService: EmployeeService, private router: Router) {}
  resolve(
    route: ActivatedRouteSnapshot
  ): Observable<PaginatedResult<EmployeeList[]>> {

    this.empParams.deleted = true;
    return this.empService
      .getDeletedEmps(this.pageNumber, this.pageSize, this.empParams)
      .pipe();
  }
}

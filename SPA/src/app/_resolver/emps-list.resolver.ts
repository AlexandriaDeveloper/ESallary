import { PaginatedResult } from '../_models/Pagination';
import { EmployeeService } from '../_services/employee.service';
import { Injectable } from '@angular/core';
import { Resolve, Router, ActivatedRouteSnapshot } from '@angular/router';
import { Observable } from 'rxjs';
import { EmployeeList } from '../_models/employee.model';

@Injectable({
  providedIn: 'root'
})
export class EmpsListResolver implements Resolve<PaginatedResult<EmployeeList[]>> {
  pageNumber = 1;
  pageSize = 30;
  constructor(private empService: EmployeeService, private router: Router) {}
  resolve(route: ActivatedRouteSnapshot): Observable<PaginatedResult<EmployeeList[]>> {
    return this.empService.getEmps(this.pageNumber, this.pageSize).pipe();
  }
}

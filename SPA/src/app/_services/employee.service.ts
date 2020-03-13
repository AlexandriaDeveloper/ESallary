import { EmpParams } from './../_models/employee.model';
import { PaginatedResult } from './../_models/Pagination';
import { environment } from './../../environments/environment';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map, catchError } from 'rxjs/operators';
import { Observable, throwError } from 'rxjs';
import { EmployeeList, EmployeeDetailsModel } from '../_models/employee.model';

@Injectable({
  providedIn: 'root'
})
export class EmployeeService {
  constructor(private http: HttpClient) {} /**getAllEmployees */
  private url = environment.apiUrl + 'Employees/';
  getEmps(
    page?,
    itemsPerPage?,
    empParams?: EmpParams
  ): Observable<PaginatedResult<EmployeeList[]>> {
    const paginatedResult: PaginatedResult<
      EmployeeList[]
    > = new PaginatedResult<EmployeeList[]>();
    let params: HttpParams = new HttpParams();
    if (empParams != null) {
      params = this.initializeParams(empParams);
    }
    if (page != null && itemsPerPage != null) {
      params = params.append('pageNumber', page);
      params = params.append('pageSize', itemsPerPage);
    }

    return this.http
      .get<any[]>(this.url + 'getAllEmployees', { observe: 'response', params })
      .pipe(
        map(response => {
          paginatedResult.result = response.body;
          if (response.headers.get('Pagination') != null) {
            paginatedResult.pagination = JSON.parse(
              response.headers.get('Pagination')
            );
          }
          return paginatedResult;
        }),
        catchError((err: any) => {
          return throwError(err);
        })
      );
  }
  getDeletedEmps(
    page?,
    itemsPerPage?,
    empParams2?
  ): Observable<PaginatedResult<EmployeeList[]>> {
    return this.getEmps(page, itemsPerPage, empParams2);
  }
  getEmpsDebounced(page?, itemsPerPage?, empParams?) {
    return this.getEmps(page, itemsPerPage, empParams).pipe(
      catchError((err: any) => {
        return throwError(err);
      })
    );
  }
  getAllEmps() {
    return this.http.get(this.url + 'Employees').pipe(
      catchError((err: any) => {
        return throwError(err);
      })
    );
  }

  getAddEmps() {
    return this.http.get(this.url + 'getAddEmployeeData').pipe(
      catchError((err: any) => {
        return throwError(err);
      })
    );
  }
  getEmpDetails(id): Observable<EmployeeDetailsModel> {
    return this.http
      .get<EmployeeDetailsModel>(this.url + 'getEmployeeDetails/' + id)
      .pipe(
        catchError((err: any) => {
          return throwError(err);
        })
      );
  }
  getEmpFinicial(id): Observable<any> {
    return this.http
      .get<any>(environment.apiUrl + 'FinincialAccount/' + id)
      .pipe(
        catchError((err: any) => {
          return throwError(err);
        })
      );
  }
  getEmpByName(name = ''): Observable<any> {
    let params: HttpParams = new HttpParams();
    params = params.append('empName', name);
    return this.http.get<any>(this.url + 'employeesByName/' + name).pipe(
      catchError((err: any) => {
        return throwError(err);
      })
    );
  }
  searchEmps(model) {
    return this.http.post(this.url + 'getEmployeeSearchMethod', model).pipe(
      catchError((err: any) => {
        return throwError(err);
      })
    );
  }
  AddNewEmps(model) {
    return this.http.post(this.url + 'addEmployee', model).pipe(
      catchError((err: any) => {
        return throwError(err);
      })
    );
  }
  AddEmpFinance(model) {
    return this.http.post(this.url + 'postEmployeeFinincialData', model).pipe(
      catchError((err: any) => {
        return throwError(err);
      })
    );
  }
  putEmps(model) {
    return this.http.put(this.url + 'putEmployeeDetails', model).pipe(
      catchError((err: any) => {
        return throwError(err);
      })
    );
  }
  postEmpsDB(model) {
    return this.http.post(this.url + 'UploadEmployees', JSON.parse(model)).pipe(
      catchError((err: any) => {
        return throwError(err);
      })
    );
  }
  deleteOrRestoreEmployee(id: string, state: boolean) {
    return this.http
      .patch(this.url + 'deleteEmployee/' + id + '/' + state, null)
      .pipe(
        catchError((err: any) => {
          return throwError(err);
        })
      );
  }
  deleteEmployee(id: string) {
    return this.http.delete(this.url + 'deleteEmployeePermenatly/' + id).pipe(
      catchError((err: any) => {
        return throwError(err);
      })
    );
  }
  getCollage() {
    return this.http.get(this.url + 'getCollages').pipe(
      catchError((err: any) => {
        return throwError(err);
      })
    );
  }
  getDepartment() {
    return this.http.get(this.url + 'getDepartments').pipe(
      catchError((err: any) => {
        return throwError(err);
      })
    );
  }
  getGradre() {
    return this.http.get(this.url + 'getGrades').pipe(
      catchError((err: any) => {
        return throwError(err);
      })
    );
  }

  public initializeParams(param: any): HttpParams {
    let params = new HttpParams();
    const objProps = Object.getOwnPropertyNames(param);
    objProps.forEach(propId => {
      const prop = Object.getOwnPropertyDescriptor(param, propId);
      if (prop.value !== '') {
        params = params.append(propId, prop.value);
      } else {
        params = params.delete(propId);
      }
    });

    return params;
  }
}

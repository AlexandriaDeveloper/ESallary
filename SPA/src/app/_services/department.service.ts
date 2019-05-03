import { catchError } from 'rxjs/operators';
import { throwError, Observable } from 'rxjs';
import { DepartmentModel } from './../_models/employee.model';
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class DepartmentService {
  constructor(private http: HttpClient) {}
  private url = environment.apiUrl + 'Department/';
  getDepartments(): Observable<DepartmentModel[]> {
    return this.http.get<DepartmentModel[]>(this.url).pipe(
      catchError(err => {
        return throwError(err);
      })
    );
  }
  getDepartment(id): Observable<DepartmentModel> {
    return this.http.get<DepartmentModel>(`${this.url}${id}`).pipe(
      catchError(err => {
        return throwError(err);
      })
    );
  }
}

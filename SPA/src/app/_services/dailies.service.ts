import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { catchError, map } from 'rxjs/operators';
import { throwError } from 'rxjs';
import { environment } from 'src/environments/environment';
import { EmpParams } from '../_models/employee.model';
import { PaginatedResult } from '../_models/Pagination';

@Injectable({
  providedIn: 'root'
})
export class DailiesService {
  constructor(private http: HttpClient) {}
  private fileUrl = environment.apiUrl + 'daily/';
  getFile(page?, itemsPerPage?, empParams?: EmpParams) {
    const paginatedResult: PaginatedResult<any[]> = new PaginatedResult<
      any[]
    >();
    let params: HttpParams = new HttpParams();
    if (page != null && itemsPerPage != null) {
      params = params.append('pageNumber', page);
      params = params.append('pageSize', itemsPerPage);
    }
    return this.http.get(this.fileUrl, { observe: 'response', params }).pipe(
      map((response: any) => {
        paginatedResult.result = response.body;
        if (response.headers.get('Pagination') != null) {
          paginatedResult.pagination = JSON.parse(
            response.headers.get('Pagination')
          );
        }
        console.log(response);
        return paginatedResult;
      }),
      catchError((err: any) => {
        return throwError(err);
      })
    );
  }
}

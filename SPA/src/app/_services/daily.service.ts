import { Injectable } from '@angular/core';
import {
  HttpClient,
  HttpHeaders,
  HttpParams,
  HttpResponse
} from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { catchError, map } from 'rxjs/operators';
import { throwError, Observable } from 'rxjs';
import { EmpParams } from '../_models/employee.model';
import { PaginatedResult } from '../_models/Pagination';

@Injectable({
  providedIn: 'root'
})
export class DailyService {
  constructor(private http: HttpClient) {} /**getAllEmployees */
  private fileUrl = environment.apiUrl + 'File/';
  // private fileTypeUrl = environment.apiUrl + 'FileType/';
  getFiles(
    page?,
    itemsPerPage?,
    empParams?: EmpParams
  ): Observable<PaginatedResult<any[]>> {
    const paginatedResult: PaginatedResult<any[]> = new PaginatedResult<
      any[]
    >();
    let params: HttpParams = new HttpParams();
    if (empParams != null) {
      params = this.initializeParams(empParams);
    }
    if (page != null && itemsPerPage != null) {
      params = params.append('pageNumber', page);
      params = params.append('pageSize', itemsPerPage);
    }
    return this.http
      .get<any[]>(this.fileUrl, { observe: 'response', params })
      .pipe(
        map(response => {
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
  getFile(id) {
    return this.http.get(this.fileUrl + 'id').pipe(
      catchError((err: any) => {
        return throwError(err);
      })
    );
  }
  getFileTypeList() {
    return this.http.get(this.fileUrl + 'getFileTypeList').pipe(
      catchError((err: any) => {
        return throwError(err);
      })
    );
  }
  putFile(model) {
    return this.http.put(this.fileUrl, model).pipe(
      catchError((err: any) => {
        return throwError(err);
      })
    );
  }
  getAllSuggestedEmps(name, paymentType) {
    let params = new HttpParams();
    params = params.append('name', name);
    params = params.append('paymentType', paymentType);
    return this.http
      .get(this.fileUrl + 'SuggestAllEmployee', { observe: 'response', params })
      .pipe(
        catchError((err: any) => {
          return throwError(err);
        })
      );
  }

  getSheetsName(sheet) {
    return this.http.post(this.fileUrl, sheet).pipe(
      catchError((err: any) => {
        return throwError(err);
      })
    );
  }
  saveSheetData(sheet) {
    return this.http.post(this.fileUrl + 'SaveSheetData', sheet).pipe(
      catchError((err: any) => {
        return throwError(err);
      })
    );
  }
  readSheets(sheet) {
    console.log(sheet);
    let model = {
      sheets: sheet.selectedSheets,
      path: sheet.path,
      paymentType: sheet.paymentType
    };
    return this.http.post(this.fileUrl + 'readSheets', model).pipe(
      catchError((err: any) => {
        return throwError(err);
      })
    );
  }
  deleteEmployeeFileDetail(model) {
    return this.http.post(this.fileUrl + 'deleteData/', model).pipe(
      catchError((err: any) => {
        return throwError(err);
      })
    );
  }

  deleteFile(id) {
    return this.http.delete(this.fileUrl + 'deleteFile/' + id).pipe(
      catchError((err: any) => {
        return throwError(err);
      })
    );
  }
  // AddNewFiles(model) {
  //   console.log(model);
  //   const options = {
  //     headers: new HttpHeaders({
  //       'Content-Type':
  //         'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet'
  //     })
  //   };
  //   return this.http.post(this.fileUrl + 'getSheetsName/', model, options).pipe(
  //     catchError((err: any) => {
  //       return throwError(err);
  //     })
  //   );
  // }

  downloadFile(id, fileType) {
    return this.http
      .get(this.fileUrl + `downloadFile/${id}/${fileType}`, {
        responseType: 'blob'
      })
      .pipe(
        // map(
        //   (res:any) => {
        //         const blob = new Blob([res.body], {type: 'application/vnd.ms-excel'} )
        //         return blob;
        //   }),
        catchError((err: any) => {
          return throwError(err);
        })
      );
  }
  printFile(id, fileType) {
    return this.http.get(this.fileUrl + `printFile/${id}/${fileType}`).pipe(
      // map(
      //   (res:any) => {
      //         const blob = new Blob([res.body], {type: 'application/vnd.ms-excel'} )
      //         return blob;
      //   }),
      catchError((err: any) => {
        return throwError(err);
      })
    );
  }
  private initializeParams(param: any): HttpParams {
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

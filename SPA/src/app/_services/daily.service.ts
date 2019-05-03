import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { catchError } from 'rxjs/operators';
import { throwError } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class DailyService {
  constructor(private http: HttpClient) {} /**getAllEmployees */
  private fileUrl = environment.apiUrl + 'File/';
  // private fileTypeUrl = environment.apiUrl + 'FileType/';
  getFiles() {
    return this.http.get(this.fileUrl).pipe(
      catchError((err: any) => {
        return throwError(err);
      })
    );
  }
  getFile(id) {
    return this.http.get(this.fileUrl + id).pipe(
      catchError((err: any) => {
        return throwError(err);
      })
    );
  }
  // getSheetsName(sheet) {
  //   return this.http.post(this.fileUrl, sheet).pipe(
  //     catchError((err: any) => {
  //       return throwError(err);
  //     })
  //   );
  // }
  // getSheetsName(sheet) {
  //   return this.http.post(this.fileUrl, sheet).pipe(
  //     catchError((err: any) => {
  //       return throwError(err);
  //     })
  //   );
  // }
  getSheetsName(sheet) {
    return this.http.post(this.fileUrl, sheet).pipe(
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
}

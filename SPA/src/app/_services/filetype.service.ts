import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { catchError } from 'rxjs/operators';
import { throwError } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class FiletypeService {
  constructor(private http: HttpClient) {} /**getAllEmployees */
  private url = environment.apiUrl + 'FileType/';
  getAllFilesType() {
    return this.http.get(this.url).pipe(
      catchError((err: any) => {
        return throwError(err);
      })
    );
  }
  postFileType(model) {
    return this.http.post(this.url, model).pipe(
      catchError((err: any) => {
        return throwError(err);
      })
    );
  }
}

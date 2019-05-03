import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { HttpClient } from '@angular/common/http';
import { FinincialAccount } from '../_models/finincial-account.model';
import { catchError } from 'rxjs/operators';
import { throwError, Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class FinicialAccountService {
  private url = environment.apiUrl + 'FinincialAccount/';
  constructor(private http: HttpClient) {}
  getAccounts(): Observable<FinincialAccount[]> {
    return this.http.get<FinincialAccount[]>(this.url).pipe(
      catchError(err => {
        return throwError(err);
      })
    );
  }
}

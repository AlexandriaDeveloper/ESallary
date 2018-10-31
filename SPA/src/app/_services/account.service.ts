import { environment } from './../../environments/environment';
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class AccountService {
  url = environment.apiUrl + 'Auth/';
  constructor(private http: HttpClient) {}

  loginUser(model) {
    return this.http.post(this.url + 'login', model);
  }
}

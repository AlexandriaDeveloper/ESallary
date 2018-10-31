import { environment } from './../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class EmployeeService {
  constructor(private http: HttpClient) {}
  private url = environment.apiUrl + 'Employees/';
  getEmps() {
    return this.http.get(this.url + 'get');
  }
}

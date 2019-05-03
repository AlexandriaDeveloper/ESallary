import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class BankService {
  private url = environment.apiUrl + 'Bank/';
  constructor(private http: HttpClient) {}
  getAllBanks() {
    return this.http.get(this.url + 'getBank');
  }
  getAllBankBranchess(id) {
    return this.http.get(this.url + 'getBankBranch/' + id);
  }
  addBank(model) {
    return this.http.post(this.url + 'newBank', model);
  }
  addBranch(model) {
    return this.http.post(this.url + 'newBranch', model);
  }
}

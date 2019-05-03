import { Injectable } from '@angular/core';
import { _ } from 'underscore';
import { EmployeeDetailsModel } from '../_models/employee.model';
@Injectable({
  providedIn: 'root'
})
export class EmployeeSharedService {
  paymentOptionList: string[] = ['حواله صراف', 'كود مؤسسى'];

  constructor() {}
  getPaymentList(): string[] {
    return this.paymentOptionList;
  }
  pushPaymentList(paymentName: string) {
    return this.paymentOptionList.push(paymentName);
  }
  removePaymentList(paymentName: string) {

    return  _.without(this.paymentOptionList, paymentName);
  }
}

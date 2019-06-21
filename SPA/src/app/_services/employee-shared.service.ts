import { Injectable } from '@angular/core';
import { _ } from 'underscore';
import { EmployeeDetailsModel } from '../_models/employee.model';
@Injectable({
  providedIn: 'root'
})
export class EmployeeSharedService {
  paymentOptionList: string[] = ['حواله صراف'];

  constructor() {}
  getPaymentList(): string[] {
    return this.paymentOptionList;
  }
  pushPaymentList(paymentName: string) {
    return this.paymentOptionList.push(paymentName);
  }
  removePaymentList(paymentName: string) {
    return _.without(this.paymentOptionList, paymentName);
  }
  getEmployeePaymentList(emp): string[] {
    this.paymentOptionList= ['حواله صراف'];
    if (emp.hasATM) {
      this.pushPaymentList('2-اخرى بطاقات حكومية');
    }
    if (emp.hasBank) {
      this.pushPaymentList('3-مرتب تحويلات بنكية');
    }
    if (emp.hasOrder) {
      this.pushPaymentList('أمر دفع');
    }
    if (emp.hasPost) {
      this.pushPaymentList('حواله بريديه أشخاص');
    }
    return this.paymentOptionList;
  }
}

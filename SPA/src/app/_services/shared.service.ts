import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class SharedService {
  private _paymetMethod = [
    'الباب الأول',
    'الباب الثانى',
    'الباب الثالث',
    'الباب الرابع',
    'الباب الخامس',
    'الباب السادس',
    'دائن',
    'صندوق خدمه تعليميه',
    'البرنامج الدولى ',
    'صيدله أكلينيكيه'
  ];
  private _collageName = ['طب بشرى', 'طب أسنان', 'كليه التمريض', 'صيدله'];
  private _paymentType = [
    '3-مرتب تحويلات بنكية',
    '2-اخرى بطاقات حكومية',
    'أمر دفع',
    'حواله بريديه أشخاص',
    'حواله صراف'
  ];
  constructor() {}

  paymetMethod(): string[] {
    return this._paymetMethod;
  }
  paymentType(): string[] {
    return this._paymentType;
  }

  getPaymentType(i): string {
    let result = '';
    if (i === 'atm') {
      result = this._paymentType.find(x => x === '2-اخرى بطاقات حكومية');
    }
    if (i === 'bank') {
      result = this._paymentType.find(x => x === '3-مرتب تحويلات بنكية');
    }
    if (i === 'order') {
      result = this._paymentType.find(x => x === 'أمر دفع');
    }
    if (i === 'personalPost') {
      result = this._paymentType.find(x => x === 'حواله بريديه أشخاص');
    }
    if (i === 'internalPost') {
      result = this._paymentType.find(x => x === 'حواله صراف');
    }
    return result;
  }
  collageName(): string[] {
    return this._collageName;
  }
}

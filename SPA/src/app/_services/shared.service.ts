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
  private _paymentType = ['3-مرتب تحويلات بنكية', '2-اخرى بطاقات حكومية'];
  constructor() {}

  paymetMethod(): string[] {
    return this._paymetMethod;
  }
  paymetType(): string[] {
    return this._paymentType;
  }
  collageName(): string[] {
    return this._collageName;
  }
}

import { CanDeactivate } from '@angular/router';
import { Injectable } from '@angular/core';
import { DailyFileComponent } from '../daily/daily-file/daily-file.component';

@Injectable({
  providedIn: 'root'
})
export class PreventFileUnsavedChangedService
  implements CanDeactivate<DailyFileComponent> {
  constructor() {}

  canDeactivate(component: DailyFileComponent) {

    if (component.fileForm.dirty && !component.fileForm.submitted) {
      return confirm('هل انت متأكد ؟؟؟ البيانات لم يتم حفظها ');
    }
    return true;
  }
}

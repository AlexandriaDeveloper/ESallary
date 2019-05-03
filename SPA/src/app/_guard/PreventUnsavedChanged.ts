import { EmployeeDetailsComponent } from './../employees/employees-list/employee-details/employee-details.component';

import { Injectable } from '@angular/core';
import { CanDeactivate } from '@angular/router';

@Injectable()
export class PreventUnsavedChanged
  implements CanDeactivate<EmployeeDetailsComponent> {
  canDeactivate(component: EmployeeDetailsComponent) {
    if (component.editForm.dirty) {
      return confirm('هل انت متأكد ؟؟؟ البيانات لم يتم حفظها ');
    }
    return true;
  }
}

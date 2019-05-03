import { RestoreDialogComponent } from './../../../../../_components/restore-dialog/restore-dialog.component';
import { EmployeeDetailsModel } from './../../../../../_models/employee.model';
import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { DeleteDialogComponent } from 'src/app/_components/delete-dialog/delete-dialog.component';

@Component({
  // tslint:disable-next-line:component-selector
  selector: '[app-employee-restore-row]',
  template: `
    <ng-content></ng-content>
    <td scope="col-1">{{ emp.grade }}</td>
    <td scope="col-1">{{ emp.collage }}</td>
    <td scope="col-1">{{ emp.id }}</td>
    <td scope="col-1">{{ emp.code }}</td>
    <td scope="col-2">{{ emp.name }}</td>
    <td scope="col-1">
      <button
        (click)="deletePermenatly()"
        class="btn btn-danger rounded-left"
        tooltip="حذف بيانات المستخدم نهائيا "
      >
        <i class="fa fa-trash-o"></i>
      </button>
      <button
        (click)="restoreEmp()"
        class="btn btn-success rounded-right"
        tooltip="أستعادة بيانات الموظف على المنظومه"
      >
        <i class="fa fa-recycle"></i>
      </button>
    </td>
  `
})
export class EmployeeRestoreRowComponent implements OnInit {
  // tslint:disable-next-line:no-input-rename
  @Input('app-employee-restore-row')
  emp: EmployeeDetailsModel;
  modalRef: BsModalRef;
  constructor(private modalService: BsModalService) {}
  @Output()
  delete: EventEmitter<EmployeeDetailsModel> = new EventEmitter<
    EmployeeDetailsModel
  >();
  @Output()
  restore: EventEmitter<EmployeeDetailsModel> = new EventEmitter<
    EmployeeDetailsModel
  >();
  ngOnInit() {}
  deletePermenatly() {
    this.modalRef = this.modalService.show(DeleteDialogComponent, {
      initialState: { message: `هل تريد حذف ${this.emp.name}` }
    });
    this.modalRef.content.onClose.subscribe(res => {
      if (res) {
        this.delete.emit(this.emp);
      }
      this.modalRef.hide();
    });
  }
  restoreEmp() {
    this.modalRef = this.modalService.show(RestoreDialogComponent, {
      initialState: { message: `هل تريد أستعادة بيانات ${this.emp.name}` }
    });
    this.modalRef.content.onClose.subscribe(res => {
      if (res) {
        this.restore.emit(this.emp);
      }
      this.modalRef.hide();
    });
  }
}

import { EmployeeDetailsModel } from 'src/app/_models/employee.model';
import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { BsModalService, BsModalRef } from 'ngx-bootstrap/modal';
import { DeleteDialogComponent } from 'src/app/_components/delete-dialog/delete-dialog.component';

@Component({
  // tslint:disable-next-line:component-selector
  selector: '[app-employee-list-row]',
  template: `

    <td class="">
      <div class=" btn-group btn-group-sm">
        <a
          class=" btn btn-primary rounded-right"
          [routerLink]="['/emps/details', item.id]"
        >
          تفاصيل <i class="fa fa-info-circle"></i>
        </a>

        <button class=" btn btn-danger rounded-left" (click)="deleteHandler()">
          حذف <i class="fa fa-trash-o" aria-hidden="true"></i>
        </button>
      </div>
    </td>
    <td class="">{{ item.nationalId }}</td>
    <td class="">{{ item.name }}</td>
    <td class="">{{ item.department }}</td>
    <td class="">{{ item.grade }}</td>
    <td class="">{{ item.collage }}</td>
    <td class="">
      <div *ngIf="item.gender == 'Male'">ذكر</div>
      <div *ngIf="item.gender == 'Female'">أنثى</div>
    </td>
    <td class="">
      <span *ngIf="item.employeePaymentType.hasATM === true"> بطاقه </span>
      <span *ngIf="item.employeePaymentType.hasBank === true"> , بنك </span>
      <span *ngIf="item.employeePaymentType.hasOrder === true">
        , أمر دفع
      </span>

      <span *ngIf="item.employeePaymentType.hasPost === true"> , حواله </span>
    </td>
    <td class="">{{ item.code }}</td>
  `
})
export class EmployeeListRowComponent implements OnInit {
  @Input()
  item: EmployeeDetailsModel;
  modalRef: BsModalRef;

  @Output()
  deleteItem: EventEmitter<EmployeeDetailsModel> = new EventEmitter<
    EmployeeDetailsModel
  >();

  @Output()
  selectItem: EventEmitter<EmployeeDetailsModel> = new EventEmitter<
    EmployeeDetailsModel
  >();
  constructor(private modalService: BsModalService) {}

  ngOnInit() {}
  deleteHandler() {
    this.modalRef = this.modalService.show(DeleteDialogComponent, {
      initialState: { message: `هل تريد حذف ${this.item.name}` }
    });
    this.modalRef.content.onClose.subscribe(res => {
      console.log(res);
      if (res) {
        this.deleteItem.emit(this.item);
      }
      this.modalRef.hide();
    });
  }
}

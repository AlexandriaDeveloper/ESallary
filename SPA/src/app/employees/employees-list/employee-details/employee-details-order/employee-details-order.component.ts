import { config } from 'rxjs';
import {
  Component,
  OnInit,
  Input,
  SimpleChanges,
  OnChanges,
  Output,
  EventEmitter
} from '@angular/core';
import { EmployeeDetailsModel } from 'src/app/_models/employee.model';
import { trigger, transition, useAnimation } from '@angular/animations';
import { fadeOut, fadeIn } from 'ngx-animate';
import { BsModalService, BsModalRef } from 'ngx-bootstrap/modal';
import { EmployeeDetailsAddOrderModalComponent } from './employee-details-add-order-modal/employee-details-add-order-modal.component';
import { EmployeeSharedService } from 'src/app/_services/employee-shared.service';
import { DepartmentService } from 'src/app/_services/department.service';

@Component({
  selector: 'app-employee-details-order',
  templateUrl: './employee-details-order.component.html',
  styleUrls: ['./employee-details-order.component.css'],
  animations: [
    trigger('bankAnimation', [
      transition(
        '* => void',
        useAnimation(fadeOut, {
          params: {
            timing: 1
          }
        })
      ),
      transition(
        'void => *',
        useAnimation(fadeIn, {
          params: {
            timing: 1
          }
        })
      )
    ])
  ]
})
export class EmployeeDetailsOrderComponent implements OnInit {
  @Input() emp: EmployeeDetailsModel;
  @Output() orderState = new EventEmitter<boolean>();
  config = {
    backdrop: true,
    ignoreBackdropClick: true
  };
  modalRef: BsModalRef;
  constructor(
    private modalService: BsModalService,
    private employeeSharedService: EmployeeSharedService
  ) {}

  ngOnInit() {
    console.log(this.emp);
  }

  openModal() {
    this.modalRef = this.modalService.show(
      EmployeeDetailsAddOrderModalComponent,
      {
        initialState: {
          emp: this.emp
        },
        ignoreBackdropClick: true,
        keyboard: true
      }
    );
    this.modalRef.content.onClose.subscribe(result => {
      console.log(result);
      this.emp = result;
      this.employeeSharedService.pushPaymentList('امر دفع');
    });
  }
  deleteBankSequance() {
    this.emp.hasOrder = false;
    this.emp.order = null;
    this.employeeSharedService.removePaymentList('امر دفع');
    console.log(this.employeeSharedService.getPaymentList());
  }
}

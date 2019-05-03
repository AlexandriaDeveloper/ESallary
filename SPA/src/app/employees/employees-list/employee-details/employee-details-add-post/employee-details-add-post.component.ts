import { EmployeeDetailsAddPostModalComponent } from './employee-details-add-post-modal/employee-details-add-post-modal.component';
import { Component, OnInit, Input } from '@angular/core';
import { EmployeeDetailsModel } from 'src/app/_models/employee.model';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { EmployeeSharedService } from 'src/app/_services/employee-shared.service';
import { trigger, transition, useAnimation } from '@angular/animations';
import { fadeOut, fadeIn } from 'ngx-animate';

@Component({
  selector: 'app-employee-details-add-post',
  templateUrl: './employee-details-add-post.component.html',
  styleUrls: ['./employee-details-add-post.component.css'],
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
export class EmployeeDetailsAddPostComponent implements OnInit {
  @Input() emp: EmployeeDetailsModel;
  config = {
    backdrop: true,
    ignoreBackdropClick: true
  };
  modalRef: BsModalRef;
  constructor(
    private modalService: BsModalService,
    private employeeSharedService: EmployeeSharedService
  ) {}

  ngOnInit() {}
  openModal() {
    this.modalRef = this.modalService.show(
      EmployeeDetailsAddPostModalComponent,
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
      this.employeeSharedService.pushPaymentList('حوالة أشخاص');
    });
  }
  deletePostSequance() {
    this.emp.hasPost = false;
    this.emp.post = null;
    this.employeeSharedService.removePaymentList('حوالة أشخاص');
    console.log(this.employeeSharedService.getPaymentList());
  }
}

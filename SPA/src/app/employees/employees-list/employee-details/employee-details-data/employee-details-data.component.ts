import { DepartmentModel } from './../../../../_models/employee.model';
import { DepartmentService } from './../../../../_services/department.service';
import { EmployeeSharedService } from './../../../../_services/employee-shared.service';
import { Component, OnInit, Input, AfterViewInit } from '@angular/core';
import { EmployeeDetailsModel } from 'src/app/_models/employee.model';
import { trigger, transition, useAnimation } from '@angular/animations';
import { bounceInDown } from 'ngx-animate';
import { INgxSelectOption } from 'ngx-select-ex';

@Component({
  selector: 'app-employee-details-data',
  templateUrl: './employee-details-data.component.html',
  styleUrls: ['./employee-details-data.component.css'],
  animations: [
    trigger('titleAnimation', [
      transition(
        '* => *',
        useAnimation(bounceInDown, {
          params: {
            timing: 3,

            // Specify granular values for `translate` on axis Y during 'bounceInDown'
            a: '-3000px',
            b: '25px',
            c: '-10px',
            d: '5px'
          }
        })
      )
    ])
  ]
})
export class EmployeeDetailsDataComponent implements OnInit, AfterViewInit {
  @Input() emp: EmployeeDetailsModel;
  @Input() departments: DepartmentModel[];
  @Input() bank: boolean;
  paymentOptionsList: string[];
  public selectedOptions: INgxSelectOption;
  constructor(private employeeSharedService: EmployeeSharedService) {}

  ngOnInit() {
    console.log(this.emp);
    if (this.emp.hasATM) {
      this.employeeSharedService.pushPaymentList('2-اخرى بطاقات حكومية');
    }
    if (this.emp.hasBank) {
      this.employeeSharedService.pushPaymentList('3-مرتب تحويلات بنكية');
    }
    if (this.emp.hasOrder) {
      this.employeeSharedService.pushPaymentList('أمر دفع');
    }
    if (this.emp.hasPost) {
      this.employeeSharedService.pushPaymentList('حواله بريديه أشخاص');
    }
    this.paymentOptionsList = this.employeeSharedService.getPaymentList();
  }
  ngAfterViewInit(): void {}
  doSelectionChanges(options: INgxSelectOption) {
    this.emp.department = options[0].data;
    console.log(this.emp);
  }
}

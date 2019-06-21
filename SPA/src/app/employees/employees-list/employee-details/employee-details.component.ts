import { EmployeeService } from './../../../_services/employee.service';
import {
  EmployeeDetailsModel,
  DepartmentModel
} from './../../../_models/employee.model';
import { Component, OnInit, AfterViewInit, ViewChild } from '@angular/core';

import { ActivatedRoute, Router } from '@angular/router';
import { trigger, useAnimation, transition } from '@angular/animations';
import { bounceInDown, fadeOut, fadeIn } from 'ngx-animate';

import { BsModalRef } from 'ngx-bootstrap/modal';

import { FormControl } from '@angular/forms';
import { CustomToastrService } from 'src/app/_services/toastr.service';
import { DepartmentService } from 'src/app/_services/department.service';
// tslint:disable-next-line:import-spacing

@Component({
  selector: 'app-employee-details',
  templateUrl: './employee-details.component.html',
  styleUrls: ['./employee-details.component.css'],
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
export class EmployeeDetailsComponent implements OnInit, AfterViewInit {
  @ViewChild('f') editForm: FormControl;
  public selectControl = new FormControl(1);

  modalRefAddBank: BsModalRef;
  modalRefAddBranch: BsModalRef;

  bankId: any;
  bankEdit = false;
  config = {
    backdrop: true,
    ignoreBackdropClick: true,
    keyboard: true
  };
  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private employeeService: EmployeeService,
    private departmentService: DepartmentService,
    private toast: CustomToastrService
  ) {}
  emp: EmployeeDetailsModel;
  departments: DepartmentModel[];
  title;
  deletedOrder;
  ngOnInit() {
    this.loadEmpData();
    this.departmentService
      .getDepartments()
      .subscribe((res: DepartmentModel[]) => {
        return (this.departments = res);
      });
  }

  loadEmpData() {
    this.route.data.subscribe(
      data => {
        this.emp = data['emp'];
        console.log(this.emp);
        this.title = data.titel;

        this.selectControl.valueChanges.subscribe(
          (subscriptionTypeId: number) => {
            console.log('subscriptionTypeId', subscriptionTypeId);
          }
        );
      },
      err => {},
      () => {}
    );
  }
  ngAfterViewInit(): void {
    console.log(this.departments);
  }
  test() {
    console.log(this.emp);
    this.employeeService.putEmps(this.emp).subscribe(
      x => {
        this.toast.showSuccess('تم حفظ البيان بنجاح');
      },
      err => {
        this.toast.showError('عفوا حدث خطأ اثناء الحفظ');
      },
      () => {}
    );
  }
  finance() {
    console.log('navigate');
    this.router.navigate([`emps/details/${this.emp.id}/finance`]);

  }
}

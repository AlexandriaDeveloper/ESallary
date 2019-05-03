import { DepartmentService } from './../../_services/department.service';
import { _ } from 'underscore';

import {
  EmployeeDetailsModel,
  EmployeeToAdd
} from './../../_models/employee.model';
import { CustomToastrService } from './../../_services/toastr.service';
import { EmployeeService } from './../../_services/employee.service';
import { Component, OnInit, ViewChild } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { NgxSelectComponent, NgxSelectOption } from 'ngx-select-ex';
import { trigger, transition, useAnimation } from '@angular/animations';
import { rotateIn } from 'ngx-animate';
import { Router } from '@angular/router';

@Component({
  selector: 'app-employee-add',
  templateUrl: './employee-add.component.html',
  styleUrls: ['./employee-add.component.css'],
  animations: [
    trigger('titleAnimation', [
      transition(
        '* => *',
        useAnimation(rotateIn, {
          params: {
            timing: 1
          }
        })
      )
    ])
  ]
})
export class EmployeeAddComponent implements OnInit {
  empForm: FormGroup;
  @ViewChild('department') public ngDepartmentSelect: NgxSelectComponent;
  @ViewChild('collage') public ngCollageSelect: NgxSelectComponent;
  collageList: any;
  gradeList: any;
  departmentList: any;
  selectedDep: { id: number; name: string };
  emp: EmployeeDetailsModel = new EmployeeDetailsModel();
  constructor(
    private toast: CustomToastrService,
    private employeeService: EmployeeService,
    private departmentService: DepartmentService,
    private router: Router,
    private fb: FormBuilder
  ) {}

  data: any;
  myobj: any;
  ngOnInit() {
    this.initialForm();

    const result = this.employeeService
      .getCollage()

      .subscribe(t => {
        this.collageList = t;
      });
    this.employeeService.getGradre().subscribe(
      g => {
        this.gradeList = g;
      },
      err => {
        this.toast.showError('Error Occiurred');
      },
      () => {}
    );
    this.departmentService.getDepartments().subscribe(department => {
      this.departmentList = department;
      console.log(department);
    });
  }
  initialForm() {
    this.empForm = this.fb.group({
      name: [this.emp.name, [Validators.required]],
      id: [
        this.emp.id,
        [
          Validators.required,
          Validators.minLength(14),
          Validators.maxLength(14)
        ]
      ],
      phone: [this.emp.phone, []],
      email: [this.emp.email, [Validators.email]],
      department: [this.selectedDep, []],
      grade: [this.emp.grade, []],
      collage: [this.emp.collage, []]
    });
  }
  doSelectionChanges(event: NgxSelectOption[]) {
    if (event.length > 0) {
      this.selectedDep = {
        id: event[0].data.id,
        name: event[0].data.name
      };
    }
  }
  submitForm() {
    this.emp = Object.assign({}, this.empForm.value);
    this.emp.knownAs = this.emp.name;
    if (this.selectedDep) {
      this.emp.departmentId = this.selectedDep.id;
    }
    if (!this.emp.department) {
      this.emp.departmentId = this.departmentList[0].id;
    }

    this.employeeService.AddNewEmps(this.emp).subscribe(
      res => {
        this.router.navigate(['emps/details/' + this.emp.id]);
      },
      err => {
        this.toast.showError('يوجد خطأ' + err.error);
      }
    );
  }
  clear() {
    console.log(this.gradeList);
  }
  clearDepartment() {
    this.selectedDep = null;
  }
}

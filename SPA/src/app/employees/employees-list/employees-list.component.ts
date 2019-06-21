import { GridFlowAnimation } from './../../_animations/grid-flow-animation';
import { EmpParams } from './../../_models/employee.model';
import { DeleteDialogComponent } from './../../_components/delete-dialog/delete-dialog.component';
import { Subject, Subscription, fromEvent } from 'rxjs';
import { Pagination, PaginatedResult } from './../../_models/Pagination';
import { EmployeeService } from './../../_services/employee.service';
import { _ } from 'underscore';
import {
  Component,
  OnInit,
  ViewChild,
  ElementRef,
  AfterViewInit,
  OnDestroy
} from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { EmployeeList } from 'src/app/_models/employee.model';
import { NgxSpinnerService } from 'ngx-spinner';
import {
  ScrollToService,
  ScrollToConfigOptions
} from '@nicky-lenaers/ngx-scroll-to';

import {
  map,
  debounceTime,
  distinctUntilChanged,
  pluck,
  share
} from 'rxjs/operators';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';

@Component({
  selector: 'app-employees-list',

  templateUrl: './employees-list.component.html',
  styleUrls: ['./employees-list.component.css'],
  animations: GridFlowAnimation.animations
})
export class EmployeesListComponent
  implements OnInit, AfterViewInit, OnDestroy {
  data: EmployeeList[];
  pagination: Pagination;
  pageNumber: number;
  pageSize: number;
  maxSize = 10;
  employee: EmployeeList;
  state = 'show';
  genderList = [
    { value: 'male', display: 'ذكر', checked: true },
    { value: 'female', display: 'أنثى', checked: true }
  ];
  paymentList = [
    { value: 'hasATM', display: 'بطاقه', checked: true },
    { value: 'hasBank', display: 'بنك', checked: true },
    { value: 'hasOrder', display: 'أمر دفع', checked: true },
    { value: 'hasPost', display: 'حواله', checked: true }
  ];
  empParams: EmpParams = new EmpParams();
  inputSubject: Subject<string> = new Subject<string>();
  @ViewChild('name') nameInputRef: ElementRef;
  @ViewChild('id') idInputRef: ElementRef;
  @ViewChild('department') departmentInputRef: ElementRef;
  @ViewChild('collage') collageInputRef: ElementRef;
  @ViewChild('code') codeInputRef: ElementRef;
  @ViewChild('grade') gradeInputRef: ElementRef;
  @ViewChild('payment') paymentInputRef: ElementRef;

  el = this.nameInputRef;
  config: ScrollToConfigOptions = {
    target: 'destination-1',
    duration: 750,
    easing: 'easeOutElastic',
    offset: -90
  };
  myObservable;
  empSubscripton: Subscription;
  ctrlSubscripton: Subscription;
  modalRef: BsModalRef;
  selectedEmp;

  ngAfterViewInit(): void {
    this.subscribCtrl(this.nameInputRef.nativeElement);
    this.subscribCtrl(this.idInputRef.nativeElement);
    this.subscribCtrl(this.codeInputRef.nativeElement);
    this.subscribCtrl(this.departmentInputRef.nativeElement);
    this.subscribCtrl(this.collageInputRef.nativeElement);
    this.subscribCtrl(this.codeInputRef.nativeElement);
    this.subscribCtrl(this.gradeInputRef.nativeElement);
  }
  subscribCtrl(ctrl) {
    this.ctrlSubscripton = fromEvent(ctrl, 'keyup')
      .pipe(
        pluck('target', 'value'),
        debounceTime(600),
        distinctUntilChanged(),
        map(x => {
          this.loadUsers();
        })
      )
      .subscribe(x => {}, err => {}, () => this.ctrlSubscripton.unsubscribe());
  }
  constructor(
    private employeeService: EmployeeService,
    private route: ActivatedRoute,
    private spinner: NgxSpinnerService,
    private _scrollToService: ScrollToService,
    private modalService: BsModalService
  ) {}

  ngOnInit() {
    this.route.data.subscribe(
      data => {
        this.spinner.show();
        this.data = data['emps'].result;
        this.pagination = data['emps'].pagination;
        console.log(this.data);
        //  this.spinner.hide();
        this.spinner.hide();
      },
      err => {
        this.spinner.hide();
      },
      () => {
        this.spinner.hide();
      }
    );
  }

  pageNums(i: number) {
    return new Array(i);
  }
  pageChanged(event: any): void {
    this.pagination.currentPage = event.page;
    //  window.scrollTo(0, 0);
    this._scrollToService.scrollTo(this.config);
    // this.loadUsers();
    this.loadUsers();
  }

  loadUsers() {
    this.data = [];
    this.empSubscripton = this.employeeService
      .getEmps(
        this.pagination.currentPage,
        this.pagination.ItemsPerPage,
        this.empParams
      )
      .pipe(
        map((res: PaginatedResult<EmployeeList[]>) => {
          // this.spinner.show();
          this.data = res.result;
          this.pagination = res.pagination;
        })
      )
      .subscribe({
        next: function(res) {},
        error: function(err) {},
        complete: function() {
          if (this.empSubscripton != null) {
            this.empSubscripton.unsubscribe();
          }
        }
      });
  }
  updateGender(event) {
    this.empParams.male = this.genderList.find(x => x.value === 'male').checked;
    this.empParams.female = this.genderList.find(
      x => x.value === 'female'
    ).checked;
    this.loadUsers();
  }

  updatePayment(event2) {
    this.empParams.hasATM = this.paymentList.find(
      x => x.value === 'hasATM'
    ).checked;
    this.empParams.hasBank = this.paymentList.find(
      x => x.value === 'hasBank'
    ).checked;

    this.loadUsers();
  }

  paymentSearch(event) {
    console.log(event.target.value);
    if (event.target.value === 'بطاقه') {
      this.empParams.paymentType = '2-اخرى بطاقات حكومية';
    } else if (event.target.value === 'بنك') {
      this.empParams.paymentType = '3-مرتب تحويلات بنكية';
    } else if (event.target.value === 'دف') {
      this.empParams.paymentType = 'أوامر الدفع';
    } else if (event.target.value === '') {
      this.empParams.paymentType = '';
    }
    this.loadUsers();
  }
  ngOnDestroy(): void {
    if (this.empSubscripton != null) {
      this.empSubscripton.unsubscribe();
    }
    if (this.ctrlSubscripton != null) {
      this.ctrlSubscripton.unsubscribe();
    }
  }
  empDelete(emp) {
    this.employeeService.deleteOrRestoreEmployee(emp.id, true).subscribe(
      x => {
        this.data = _.without(this.data, emp);
        if (this.pagination.TotalItems >= 0) {
          this.pagination.TotalItems--;
        }
      },
      err => {},
      () => {}
    );
  }

  // test() {
  //   this.modalRef = this.modalService.show(DeleteDialogComponent, {
  //     initialState: { message: 'Hello World', title: '' }
  //   });
  //   this.modalRef.content.onClose.subscribe(res => {
  //     console.log(res);
  //     this.modalRef.hide();
  //   });
  // }
}

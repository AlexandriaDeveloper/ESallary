import { EmployeeRestoreRowComponent } from './employee-restore-row/employee-restore-row.component';
import { CustomToastrService } from 'src/app/_services/toastr.service';
import { EmpParams } from './../../../../_models/employee.model';
import { EmployeeService } from './../../../../_services/employee.service';
import {
  Component,
  OnInit,
  AfterViewInit,
  AfterContentInit,
  OnChanges,
  ViewChild,
  ElementRef,
  OnDestroy,
  ContentChild
} from '@angular/core';
import { Pagination } from 'src/app/_models/Pagination';
import { ActivatedRoute } from '@angular/router';
import { pluck, debounceTime, distinctUntilChanged, map } from 'rxjs/operators';
import { fromEvent, Subscription } from 'rxjs';
import { GridFlowAnimation } from 'src/app/_animations/grid-flow-animation';

@Component({
  selector: 'app-employee-restore',
  templateUrl: './employee-restore.component.html',
  styleUrls: ['./employee-restore.component.css'],
  animations: GridFlowAnimation.animations
})
export class EmployeeRestoreComponent
  implements OnInit, AfterContentInit, AfterViewInit, OnDestroy {
  @ViewChild('name', { static: false }) nameInputRef: ElementRef;
  @ViewChild('id', { static: false }) idInputRef: ElementRef;
  @ViewChild('collage', { static: false }) collageInputRef: ElementRef;
  @ViewChild('code', { static: false }) codeInputRef: ElementRef;
  @ViewChild('grade', { static: false }) gradeInputRef: ElementRef;
  @ContentChild(EmployeeRestoreRowComponent, { static: false })
  row: EmployeeRestoreRowComponent;
  emps: any[];
  empParams: EmpParams = new EmpParams();
  pagination: Pagination;
  ctrlSubscripton: Subscription;
  constructor(
    private employeeService: EmployeeService,
    private route: ActivatedRoute,
    private toast: CustomToastrService
  ) {}

  ngOnInit() {
    this.route.data.subscribe(
      data => {
        this.emps = data['emps'].result;
        this.pagination = data['emps'].pagination;
      },
      err => {},
      () => {}
    );
  }

  ngAfterViewInit(): void {
    this.subscribCtrl(this.nameInputRef.nativeElement);
    this.subscribCtrl(this.idInputRef.nativeElement);
    this.subscribCtrl(this.codeInputRef.nativeElement);
    this.subscribCtrl(this.collageInputRef.nativeElement);
    this.subscribCtrl(this.codeInputRef.nativeElement);
    this.subscribCtrl(this.gradeInputRef.nativeElement);
  }
  subscribCtrl(ctrl) {
    this.ctrlSubscripton = fromEvent(ctrl, 'keyup')
      .pipe(
        pluck('target', 'value'),
        debounceTime(900),
        distinctUntilChanged(),
        map(x => {
          console.log(x);
          this.loadEmps();
        })
      )
      .subscribe();
  }

  loadEmps() {
    this.empParams.deleted = true;
    this.employeeService
      .getDeletedEmps(
        this.pagination.CurrentPage,
        this.pagination.ItemsPerPage,
        this.empParams
      )
      .subscribe(x => {
        this.emps = x.result;
        this.pagination = x.pagination;
      });
  }
  removeEmp(event) {
    this.employeeService.deleteEmployee(event.id).subscribe(x => {
      this.removeFromList(event);
      this.toast.showSuccess(`تم حذف الموظف ${event.name} نهائيا`);
    });
  }
  restoreEmp(event) {
    this.employeeService
      .deleteOrRestoreEmployee(event.id, false)
      .subscribe(x => {
        this.removeFromList(event);
        this.toast.showSuccess(`تم أستعادة بيانات الموظف ${event.name} `);
      });
  }
  removeFromList(event) {
    this.emps = this.emps.filter(x => {
      return x.id !== event.id;
    });
    this.pagination.TotalItems--;
  }
  pageNums(i: number) {
    return new Array(i);
  }
  pageChanged(event: any): void {
    this.pagination.CurrentPage = event.page;
    this.loadEmps();
  }

  ngOnDestroy(): void {
    if (this.ctrlSubscripton != null) {
      this.ctrlSubscripton.unsubscribe();
    }
  }
  ngAfterContentInit() {
    if (this.row) {
      {
        this.row.delete.subscribe(clicked => {
          console.log(clicked);
        });
      }
    }
  }
}

import { SharedService } from './../../../_services/shared.service';
import { EmployeeSharedService } from 'src/app/_services/employee-shared.service';
import { CustomToastrService } from './../../../_services/toastr.service';
import { EmployeeService } from './../../../_services/employee.service';
import { Component, OnInit } from '@angular/core';
import { BsModalRef } from 'ngx-bootstrap/modal';
import { BehaviorSubject, Subject } from 'rxjs';
import { debounceTime, distinctUntilChanged, map } from 'rxjs/operators';
import {
  FormControl,
  FormGroup,
  FormBuilder,
  Validators
} from '@angular/forms';

@Component({
  selector: 'app-emp-add-modal',
  templateUrl: './emp-add-modal.component.html',
  styleUrls: ['./emp-add-modal.component.css']
})
export class EmpAddModalComponent implements OnInit {
  emp: any;
  state: string;
  items;
  empSearch: { id?: number; code: string; name: string };
  choosenEmp: any = {};
  foundEmp: any = {employeeData : { name: '', collage: '', grade: ''} };
  selectedPaymnet = '';
  f: FormGroup;
  f2: FormGroup;
  public selectName = new FormControl();
  public selectedPaymentMethod = new FormControl();
  public searchTyped = new BehaviorSubject<string>('');
  public onClose = new Subject<any>();
  payment: any;
  selectedEmp: any;
  constructor(
    public modalRef: BsModalRef,
    private employeeService: EmployeeService,
    private toast: CustomToastrService,
    private employeeShared: EmployeeSharedService,
    private shared: SharedService,
    private fb: FormBuilder
  ) {
    this.subscribCtrl();
  }

  ngOnInit() {
    this.empSearch = { name: '', code: '' };
    this.choosenEmp.index = this.emp.index;
    this.choosenEmp.fileId = this.payment.id;
    console.log(this);
    if (this.emp.index === -1) {
      this.state = 'new';
      this.choosenEmp.net = '';
    } else {
      this.choosenEmp = this.emp;
      this.state = 'updated';

      this.selectedPaymnet = this.choosenEmp.selectedPaymentMethod;
      this.selectedPaymentMethod.setValue(
        this.choosenEmp.selectedPaymentMethod
      );
    }

    this.initilizieForm();
    this.initilizieFormSearch();
  }

  hideModal() {
    this.modalRef.hide();
  }

  subscribCtrl() {
    this.selectName.valueChanges.subscribe(value => {
      const emp = this.items.filter(x => x.id === value);
      this.empSearch = emp[0];
      this.choosenEmp.selectedPaymentMethod = value;
      this.foundEmp.selectedPaymentMethod = value;
    });
    this.searchTyped
      .pipe(
        debounceTime(600),
        distinctUntilChanged(),
        map(x => {
          this.loadEmployees(x);
        })
      )
      .subscribe();
  }
  loadEmployees(name) {
    if (name === '') {
      this.items = [];
    }
    const nameLength = name.split(' ');
    if (nameLength.length > 0 && name !== '') {
      const s = this.employeeService.getEmpByName(name).subscribe(
        x => {
          this.items = x;
        },
        err => {},
        () => {
          s.unsubscribe();
        }
      );
    }
  }
  search() {
    Object.assign(this.empSearch, this.f.value);
    const s = this.employeeService.searchEmps(this.empSearch).subscribe(
      (x:any) => {
        console.log(x);
        this.foundEmp.employeeData =  x;
        this.foundEmp.paymentMethod = x.paymentMethod;

        if (this.payment.paymentType === this.shared.getPaymentType('bank')) {
          this.foundEmp.selectedPaymentMethod = this.foundEmp.employeeData.bankOption;
          this.selectedPaymnet = this.foundEmp.bankOption;
        } else if (
          this.payment.paymentType === this.shared.getPaymentType('atm')
        ) {
          this.foundEmp.selectedPaymentMethod = this.foundEmp.employeeData.atmOption;
          this.selectedPaymnet = this.foundEmp.atmOption;
        }
        console.log(this.foundEmp);
       this.initilizieForm();
        this.selectedPaymentMethod.setValue(
          this.foundEmp.selectedPaymentMethod
        );
      },
      err => {
        this.toast.showError(err.error);
      },
      () => {
        s.unsubscribe();
      }
    );
    console.log(this.f2);
  }
  clearSearch() {
    this.items = [];
    this.selectName.setValue(null);
    this.empSearch = {
      id: null,
      name: '',
      code: ''
    };
  }
  AddEmployee() {
    console.log(this.foundEmp);


    let found2 =Object.assign(this.foundEmp, this.f2.value);
console.log(this.foundEmp);
console.log(found2);

    this.foundEmp.state = this.state;
    this.foundEmp.fileId = this.payment.id;
    this.foundEmp.index = this.choosenEmp.index;

    console.log(this.foundEmp);
    this.onClose.next(this.foundEmp);
    this.modalRef.hide();
  }
  initilizieForm() {
    this.selectedPaymentMethod.valueChanges.subscribe(
      x => (this.foundEmp.selectedPaymentMethod = x)
    );
    this.f2 = this.fb.group({
      employeeData: this.fb.group({
        name: [
          { value: this.foundEmp.employeeData.name, disabled: true },
          [Validators.required]
        ],
        collage: [{ value: this.foundEmp.employeeData.collage, disabled: true }],
        position: [{ value: this.foundEmp.employeeData.grade, disabled: true }]
      }),

      net: [this.foundEmp.net, Validators.required],

      selectedPaymentMethod: [
        this.foundEmp.selectedPaymentMethod,
        Validators.required
      ]
    });
  }
  initilizieFormSearch() {
    this.f = this.fb.group({
      code: [this.empSearch.code],
      name: ['']
    });
  }
  changePaymentMethod(ev) {
    this.selectedPaymnet = ev;
    console.log(this.foundEmp);
  }
  get employeeData(): any {
    return this.f2.get('employeeData');
  }
  setEmployeeData() {
    console.log(this.foundEmp)
    this.employeeData.setValue({
      name: this.foundEmp.employeeName,
      collage: this.foundEmp.collage,
      position: this.foundEmp.grade
    });
  }
}

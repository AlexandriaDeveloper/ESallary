// tslint:disable-next-line:max-line-length
import { EmployeeDetailsAddOrderBranchModalComponent } from './employee-details-add-order-branch/employee-details-add-order-branch.component';
import { BankService } from './../../../../../_services/bank.service';
// tslint:disable-next-line:import-spacing
// tslint:disable-next-line:max-line-length
import { EmployeeDetalisAddOrderBankModalComponent } from './employee-detalis-add-order-bank-model/employee-detalis-add-order-bank-model.component';
import { Component, OnInit, AfterViewInit } from '@angular/core';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { INgxSelectOption } from 'ngx-select-ex';
import { NgForm } from '@angular/forms';
import { Subject, config, Subscription } from 'rxjs';

@Component({
  selector: 'app-employee-details-add-order-modal',
  templateUrl: './employee-details-add-order-modal.component.html',
  styleUrls: ['./employee-details-add-order-modal.component.css']
})
export class EmployeeDetailsAddOrderModalComponent
  implements OnInit, AfterViewInit {
  title: string;
  bankList: any[];
  branchList: any[];
  selectedBank: any = {};
  selectedBankBranch: any = {};
  emp: any;
  accountNumber = '';
  public onClose = new Subject<any>();
  public selectedOptions: INgxSelectOption;
  subscriptions: Subscription[] = [];
  config = {
    backdrop: true,
    ignoreBackdropClick: true,
    initialState: { bankList: [], selectedBank: {} }
  };
  constructor(
    private modalService: BsModalService,
    public modalRef2: BsModalRef,
    public modalBank: BsModalRef,
    public modalBranchRef: BsModalRef,
    public modalPostRef: BsModalRef,
    private bankService: BankService
  ) {}
  ngOnInit() {}
  ngAfterViewInit(): void {
    console.log(this.bankList);
    if (this.bankList == null) {
      this.bankService.getAllBanks().subscribe((x: any) => {
        console.log(x);
        this.bankList = x;
      });
    }
  }
  closeModal() {
    this.modalRef2.hide();
  }
  openModalAddBank() {
    this.modalBank = this.modalService.show(
      EmployeeDetalisAddOrderBankModalComponent,
      this.config
    );
    this.modalBank.content.onClose.subscribe(result => {
      console.log(result);
      this.bankList.push(result);
    });
  }

  openModalAddBranch() {
    this.config.initialState.bankList = this.bankList;
    this.config.initialState.selectedBank = this.selectedBank;
    this.modalBranchRef = this.modalService.show(
      EmployeeDetailsAddOrderBranchModalComponent,
      this.config
    );
    this.modalBranchRef.content.onClose.subscribe(
      result => {
        this.selectedBank = result;
      },
      err => {
        console.log(err);
      },
      () => {
        console.log('completed');
      }
    );
  }

  openModalAddPost() {
    this.modalPostRef = this.modalService.show(
      EmployeeDetailsAddOrderBranchModalComponent,
      this.config
    );
    this.modalBranchRef.content.onClose.subscribe(
      result => {
      },
      err => {
        console.log(err);
      },
      () => {
        console.log('completed');
      }
    );
  }

  doSelect(value: any) {
    console.log(value);
    this.selectedBank = value.optionActive.data;
    this.selectedBank.bankId = value.optionActive.value;

    console.log(this.selectedBank);
    this.bankService
      .getAllBankBranchess(this.selectedBank.bankId)
      .subscribe((res: any) => {
        this.selectedBank.branches = res;
      });
  }
  doSelectBranch(value: any) {
    console.log(value.optionActive.data);
    this.selectedBankBranch = value.optionActive.data;
  }
  public doRemove = (value: any) => {
    this.selectedBank = {};
    this.config.initialState.selectedBank = this.selectedBank;
  }
  onSubmit(f: NgForm) {
    this.emp.hasOrder = true;

    console.log(f);
    this.emp.order = {
      orderBankCode: this.selectedBank.id,
      orderBankName: this.selectedBank.bankName,
      orderBranchCode: this.selectedBankBranch.branchCode,
      orderBranchName: this.selectedBankBranch.branchName,
      orderAccountNum: this.accountNumber
    };

    this.onClose.next(this.emp);
    this.modalRef2.hide();
  }
}

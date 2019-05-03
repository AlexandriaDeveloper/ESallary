import { ToastrService } from 'ngx-toastr';
import { BankService } from './../../../../../../_services/bank.service';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { Component, OnInit } from '@angular/core';
import { Subject } from 'rxjs';
import { BsModalRef } from 'ngx-bootstrap/modal';
import { CustomToastrService } from 'src/app/_services/toastr.service';

@Component({
  selector: 'app-employee-details-add-order-branch',
  templateUrl: './employee-details-add-order-branch.component.html',
  styleUrls: ['./employee-details-add-order-branch.component.css']
})
export class EmployeeDetailsAddOrderBranchModalComponent implements OnInit {
  bankList = [];
  addBranchForm: FormGroup;
  public onClose = new Subject<any>();
  selectedBank = {
    bankId: '',
    branchName: '',
    bankName: ''
  };
  constructor(
    public modalRef: BsModalRef,
    private fb: FormBuilder,
    private bankService: BankService,
    private toast: CustomToastrService
  ) {}

  ngOnInit() {
    console.log(this);
    this.buildForm();
  }
  buildForm() {
    this.addBranchForm = this.fb.group({
      bankId: [this.selectedBank.bankId],
      bankName: [this.selectedBank.bankName, Validators.required],
      branchName: ['', Validators.required]
    });
  }
  doSelect(bankName) {
    console.log(bankName.optionActive);
    this.selectedBank = bankName.optionActive.data;
  }
  closeModal() {
    this.modalRef.hide();
  }
  onSubmitBranch() {
    if (this.addBranchForm.valid) {
      this.selectedBank.branchName = this.addBranchForm.value.branchName;
      console.log(this.selectedBank);
      this.bankService.addBranch(this.selectedBank).subscribe(
        (res: any) => {
          console.log(res);
          this.selectedBank = res;
          this.onClose.next(res);
          this.modalRef.hide();
        },
        err => {
          this.toast.showError(err.error);
        }
      );
    }
  }
}

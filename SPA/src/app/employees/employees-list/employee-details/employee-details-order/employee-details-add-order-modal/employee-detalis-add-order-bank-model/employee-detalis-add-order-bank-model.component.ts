import { BankService } from './../../../../../../_services/bank.service';
import { Component, OnInit } from '@angular/core';
import { BsModalRef } from 'ngx-bootstrap/modal';
import { Subject } from 'rxjs';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { trigger, transition, useAnimation } from '@angular/animations';
import { fadeIn, fadeOut } from 'ngx-animate';
import { CustomToastrService } from 'src/app/_services/toastr.service';

@Component({
  selector: 'app-employee-detalis-add-order-bank-model',
  templateUrl: './employee-detalis-add-order-bank-model.component.html',
  styleUrls: ['./employee-detalis-add-order-bank-model.component.css'],
  animations: [
    trigger('errorAnimation', [
      transition(
        ':enter',
        useAnimation(fadeIn, {
          params: {
            timing: 3
          }
        })
      ),
      transition(
        ':leave',
        useAnimation(fadeOut, {
          params: {
            timing: 3
          }
        })
      )
    ])
  ]
})
export class EmployeeDetalisAddOrderBankModalComponent implements OnInit {
  bank = {};
  addBankForm: FormGroup;
  public onClose = new Subject<any>();
  constructor(
    public modalRef: BsModalRef,
    private bankService: BankService,
    private fb: FormBuilder,
    private toast: CustomToastrService
  ) {}

  ngOnInit() {
    this.creatForm();
  }
  creatForm() {
    this.addBankForm = this.fb.group(
      {
        bankName: ['', Validators.required],
        accountMinLength: [
          '',
          [
            Validators.required,
            Validators.minLength(4),
            Validators.maxLength(22),
            Validators.min(4),
            Validators.max(22)
          ]
        ],
        accountMaxLength: [
          '',
          [
            Validators.required,
            Validators.minLength(4),
            Validators.maxLength(22),
            Validators.min(4),
            Validators.max(22)
          ]
        ]
      },
      {
        validator: (fg: FormGroup) => {
          return this.matchLength(fg);
        }
      }
    );
  }
  private matchLength(g: FormGroup) {
    return <number>g.get('accountMinLength').value >
      <number>g.get('accountMaxLength').value
      ? { mismatch: true }
      : null;
  }
  closeModal() {
    this.modalRef.hide();
  }
  onSubmit() {
    console.log(this.addBankForm);

    this.bankService.addBank(this.addBankForm.value).subscribe(
      x => {
        console.log(x);
        this.onClose.next(x);
        this.modalRef.hide();
      },
      err => {
        console.log(err);
        this.toast.showError(err.error);
      }
    );
  }
}

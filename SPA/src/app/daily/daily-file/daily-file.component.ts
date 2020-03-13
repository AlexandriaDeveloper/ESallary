import { FormGroup, FormControl } from '@angular/forms';
import { FiletypeService } from './../../_services/filetype.service';
import { FiletypeModalComponent } from './filetype-modal/filetype-modal.component';
import { ToastrService } from 'ngx-toastr';
import { SharedService } from './../../_services/shared.service';
import { DailyService } from './../../_services/daily.service';
import { Component, OnInit, ViewChild } from '@angular/core';

import { environment } from 'src/environments/environment';
import { trigger, transition, useAnimation } from '@angular/animations';
import { bounceInDown } from 'ngx-animate';
import { BsModalService, BsModalRef } from 'ngx-bootstrap/modal';
import { Router } from '@angular/router';

@Component({
  selector: 'app-daily-file',
  templateUrl: './daily-file.component.html',
  styleUrls: ['./daily-file.component.css'],
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
export class DailyFileComponent implements OnInit {
  @ViewChild('fileForm',{static: false}) public fileForm: FormControl;
  file: any = {};
  paymetMethod: string[];
  collagesName: string[];
  paymetType: string[];
  modalRef: BsModalRef;
  filesType: any = [];
  constructor(
    private dailyService: DailyService,
    private sharedService: SharedService,
    private toast: ToastrService,
    private modalService: BsModalService,
    private filesTypeService: FiletypeService,
    private router: Router
  ) {}
  URL = environment.apiUrl + 'File/UploadFile';
  ngOnInit() {
    this.paymetMethod = this.sharedService.paymetMethod();
    this.paymetType = this.sharedService.paymentType();
    this.collagesName = this.sharedService.collageName();
    this.filesTypeService
      .getAllFilesType()
      .subscribe((x: any) => (this.filesType = x));
  }
  submitForm() {
    console.log(this.file);
    this.dailyService.getSheetsName(this.file).subscribe(
      (x: any) => {
        console.log(x);
        this.router.navigate([`daily/files/${x.id}`]);
      },
      err => {
        console.log(err);
        err.error.forEach(element => {
          console.log(element.errorMessage);
          this.toast.error(element.errorMessage);
        });
      }
    );
  }
  openModal() {
    this.modalRef = this.modalService.show(FiletypeModalComponent, {
      initialState: {
        //emp: this.emp
      },
      ignoreBackdropClick: true,
      keyboard: true
    });
    this.modalRef.content.onClose.subscribe(result => {
      console.log(result);
      console.log(this.filesType);

      this.filesType.push(result);
      console.log(this.filesType);
    });
  }
  onChange(event) {
    console.log(event);
    this.file.fileTypeId = event;
  }

}

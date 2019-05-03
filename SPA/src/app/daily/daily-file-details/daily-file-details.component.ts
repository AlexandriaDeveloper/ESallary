import { forEach } from '@angular/router/src/utils/collection';
import {
  FormGroup,
  FormControl,
  FormBuilder,
  Validators
} from '@angular/forms';
import { FileSheetsModalComponent } from './file-sheets-modal/file-sheets-modal.component';
import { DailyService } from './../../_services/daily.service';
import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { FileUploader, FileItem } from 'ng2-file-upload';
import { environment } from 'src/environments/environment';
import { ActivatedRoute, Router } from '@angular/router';
import { CustomToastrService } from 'src/app/_services/toastr.service';
import * as XLSX from 'xlsx';
import { BsModalService, BsModalRef } from 'ngx-bootstrap/modal';
@Component({
  selector: 'app-daily-file-details',
  templateUrl: './daily-file-details.component.html',
  styleUrls: ['./daily-file-details.component.css']
})
export class DailyFileDetailsComponent implements OnInit {
  URL = environment.apiUrl + 'File/GetSheetsName/';
  public uploader: FileUploader;
  public hasBaseDropZoneOver = false;
  uploadSuccess = false;
  file: any = {};
  sheetData = {};
  response;
  empForm: FormGroup;
  config = {
    backdrop: true,
    ignoreBackdropClick: true
  };
  modalRef: BsModalRef;
  constructor(
    private fileService: DailyService,
    private router: ActivatedRoute,
    private changeDetector: ChangeDetectorRef,
    private toast: CustomToastrService,
    private route: Router,
    private modalService: BsModalService,
    private fb: FormBuilder
  ) {}
  public fileOverBase(e: any): void {
    this.hasBaseDropZoneOver = e;
    this.uploadSuccess = false;
  }
  ngOnInit() {
    const id = this.router.snapshot.params.id;
    this.fileService.getFile(id).subscribe(x => (this.file = x));

    this.uploader = new FileUploader({
      url: this.URL,
      disableMultipart: false, // 'DisableMultipart' must be 'true' for formatDataFunction to be called.
      authToken: 'Bearer ' + localStorage.getItem('token'),
      allowedMimeType: [
        'application/vnd.ms-excel',
        'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet'
      ],
      isHTML5: true,
      removeAfterUpload: true,
      maxFileSize: 10 * 1024 * 1024
    });
    this.uploader.onAfterAddingFile = file => {
      file.withCredentials = false;
      //this.fileService.AddNewFiles().subscribe();
    };
    this.uploader.onBuildItemForm = (fileItem: any, form: any) => {
      // add custom Propety
      // form.append('name', 'mohamed');
    };

    this.hasBaseDropZoneOver = false;

    this.response = '';
    this.uploader.onProgressAll = (progress: any) => {
      this.changeDetector.detectChanges();
      return progress;
    };
    this.uploader.onSuccessItem = (t: FileItem) => {
      console.log('file uploaded');
      this.uploadSuccess = true;
    };
    this.uploader.response.subscribe(
      (res: any) => {
        this.response = JSON.parse(res);
        this.response.paymentType = this.file.paymentType;
        this.openModal(this.response);

        console.log(res);
      },
      err => {
        this.toast.showError(err.error);
      },
      () => {
        this.route.navigate(['/home']);
      }
    );
  }
  openModal(ress) {
    this.modalRef = this.modalService.show(FileSheetsModalComponent, {
      initialState: {
        res: ress
      },
      ignoreBackdropClick: true,
      keyboard: true
    });
    this.modalRef.content.onClose.subscribe(result => {
      console.log(result);
      this.sheetData = result;
      this.modalRef.hide();
    });
  }
  Test() {
    console.log(this.sheetData);
  }
  suggestedToggle(val, row) {
    row.suggestedEmployee.forEach(element => {
      element.checked = false;
    });

    val.checked = !val.checked;
    row.selectedEmp = val;
    console.log(val);
  }

  paymentToggle(payment, suggested) {
    suggested.selectedPaymentMethod = payment;
  }
}

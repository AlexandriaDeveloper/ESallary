import { EmpAddModalComponent } from './emp-add-modal/emp-add-modal.component';
import { FormGroup, FormBuilder } from '@angular/forms';
import { FileSheetsModalComponent } from './file-sheets-modal/file-sheets-modal.component';
import { DailyService } from './../../_services/daily.service';
import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { FileUploader, FileItem } from 'ng2-file-upload';
import { environment } from 'src/environments/environment';
import { ActivatedRoute, Router } from '@angular/router';
import { CustomToastrService } from 'src/app/_services/toastr.service';
import { BsModalService, BsModalRef } from 'ngx-bootstrap/modal';
import { SharedService } from 'src/app/_services/shared.service';
import { EmployeeSharedService } from 'src/app/_services/employee-shared.service';
import { GridFlowAnimation } from 'src/app/_animations/grid-flow-animation';
import {
  animate,
  transition,
  trigger,
  useAnimation
} from '@angular/animations';
import { _ } from 'underscore';
import { bounce, fadeInDown, fadeIn, slideInDown } from 'ngx-animate';
import { forEach } from '@angular/router/src/utils/collection';
@Component({
  selector: 'app-daily-file-details',
  templateUrl: './daily-file-details.component.html',
  styleUrls: ['./daily-file-details.component.css'],
  animations: GridFlowAnimation.animations,
  // animations: [
  //   trigger('bounce', [transition('* => *', useAnimation(slideInDown))])
  // ]
})
export class DailyFileDetailsComponent implements OnInit {
  URL = environment.apiUrl + 'File/GetSheetsName/';
  public uploader: FileUploader;
  public hasBaseDropZoneOver = false;
  uploadSuccess = false;
  file: any = {};
  showUpload = false;
  checkAll = false;
  sheetData: { totalSum: number; data: any[] };
  personalpost = 0;
  bank = 0;
  atm = 0;
  order = 0;
  internalpost = 0;
  response;
  empForm: FormGroup;
  config = {
    backdrop: true,
    ignoreBackdropClick: true
  };
  searchFilterData = {
    employeeName: '',
    employeeCode: '',
    paymentMethod: '',
    collage: '',
    department: '',
    position: '',
    net: ''
  };
  modalRef: BsModalRef;
  selectedEmp: { index: number; state: string };
  constructor(
    private fileService: DailyService,
    private router: ActivatedRoute,
    private changeDetector: ChangeDetectorRef,
    private toast: CustomToastrService,
    private route: Router,
    private modalService: BsModalService,
    private shared: EmployeeSharedService,
    private sharedConst: SharedService,
    private fb: FormBuilder
  ) {}
  public fileOverBase(e: any): void {
    this.hasBaseDropZoneOver = e;
    this.uploadSuccess = false;
  }
  ngOnInit() {
    const id = this.router.snapshot.params.id;
    if (this.sheetData == null) {
      this.sheetData = { totalSum: 0, data: [] };
    }

    this.fileService.getFile(id).subscribe((x: any) => {
      this.file = x;
      this.sheetData.data = x.fileDetails;
      this.calculateTotalSum();
      console.log(x);
    });

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

      // this.fileService.AddNewFiles().subscribe();
    };
    this.uploader.onBuildItemForm = (fileItem: any, form: any) => {};

    this.hasBaseDropZoneOver = false;

    this.response = '';
    this.uploader.onProgressAll = (progress: any) => {
      this.changeDetector.detectChanges();
      return progress;
    };
    this.uploader.onSuccessItem = (t: FileItem) => {
      console.log('file uploaded');
      this.uploadSuccess = true;
      this.showUpload = false;
    };
    this.uploader.response.subscribe(
      (res: any) => {
        this.response = JSON.parse(res);
        this.response.paymentType = this.file.paymentType;
        this.openModal(this.response);
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
      result.data.forEach(element => {
        element.state = 'new';
        element.paymentMethod = this.shared.getEmployeePaymentList(element);
        element.fileId = this.file.id;
        console.log(element)
        this.sheetData.data.push(element);
      });
      // this.sheetData.data +=  result.data;
      this.calculateTotalSum();
      this.modalRef.hide();
    });
  }

  newEmp() {
    this.selectedEmp = { index: -1, state: 'new' };
    this.EmployeeModal();
  }
  EmployeeModal() {
    this.modalRef = this.modalService.show(EmpAddModalComponent, {
      initialState: { emp: this.selectedEmp, payment: this.file },
      ignoreBackdropClick: true,
      keyboard: true
    });
    this.modalRef.content.onClose.subscribe(result => {
      console.log(result)
      if (result.index === -1) {

        this.sheetData.data.push(result); // += element;
      } else {
        console.log(result);
        this.sheetData.data[result.index] = result;
      }
      this.calculateTotalSum();
    });
  }
  selectEmp(row, i) {
    console.log(row);
    this.selectedEmp = row;
    this.selectedEmp.index = i;
    this.EmployeeModal();
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

  assignEmployee(s, r) {
    console.log(s);
    console.log(r);

    Object.assign(r.employeeData, s);
    Object.assign(r, s);
    r.employeeData.id = s.employeeId
    r.suggestedEmployee = null;
  }
  moreSuggestion(row) {
    this.fileService
      .getAllSuggestedEmps(row.employeeName, this.file.paymentType)
      .subscribe(x => {
        row.suggestedEmployee = x.body;
      });
  }
  save() {
    console.log(this.sheetData.data);
    this.fileService.saveSheetData(this.sheetData.data).subscribe(
      (x: any) => {
        this.sheetData.data = x;
        console.log(x);
      },
      err => {
        this.toast.showError(err.error);
      }
    );
  }
  addFile() {
    this.showUpload = !this.showUpload;
  }
  private calculateTotalSum() {
    this.sheetData.totalSum = 0;
    console.log('0' + this.sharedConst.getPaymentType('bank'));

    this.sheetData.data.forEach(element => {
      if (element.state !== 'deleted') this.sheetData.totalSum += element.net;
      if (
        element.state !== 'deleted' &&
        element.selectedPaymentMethod === this.sharedConst.getPaymentType('atm')
      ) {
        this.atm += element.net;
      }
      if (
        element.state !== 'deleted' &&
        element.selectedPaymentMethod ===
          this.sharedConst.getPaymentType('bank')
      ) {
        this.bank += element.net;
      }
      if (
        element.state !== 'deleted' &&
        element.selectedPaymentMethod ===
          this.sharedConst.getPaymentType('internalPost')
      ) {
        this.internalpost += element.net;
      }
      if (
        element.state !== 'deleted' &&
        element.selectedPaymentMethod ===
          this.sharedConst.getPaymentType('order')
      ) {
        this.order += element.net;
      }
      if (
        element.state !== 'deleted' &&
        element.selectedPaymentMethod ===
          this.sharedConst.getPaymentType('personalPost')
      ) {
        this.personalpost += element.net;
      }
      // if(element.state !== 'deleted'&& element.selectedPaymentMethod !== this.sharedConst.paymentType[3])
      // { this.bank += element.net;}
    });
    this.sheetData.totalSum = Math.round(this.sheetData.totalSum * 100) / 100;
    this.atm = Math.round(this.atm * 100) / 100;
    this.bank = Math.round(this.bank * 100) / 100;
    this.personalpost = Math.round(this.personalpost * 100) / 100;
    this.order = Math.round(this.order * 100) / 100;
    this.internalpost = Math.round(this.internalpost * 100) / 100;
  }
  // deleteEmp(row, i) {
  //   // this.sheetData.data.splice(i, 1);
  //   row.state = 'deleted';
  //   this.calculateTotalSum();
  //   console.log(this.sheetData.data);
  // }
  toggleCheckbox() {
    this.checkAll = !this.checkAll;
    this.sheetData.data.forEach(element => {
      element.checked = this.checkAll;
    });
  }
  updateFilters(): void {
    this.searchFilterData = Object.assign({}, this.searchFilterData);
    let data = [];
    data = this.sheetData.data;

    this.sheetData.data = [];
    data.forEach(element => {
      this.sheetData.data.push(element);
    });
  }
  deleteEmp() {
    let deleted;
    deleted = this.sheetData.data.filter(x => x.checked);

    this.fileService.deleteEmployeeFileDetail(deleted).subscribe(
      (x: any) => {
        deleted = x;
      },
      err => console.log(err.error)
    );
    deleted.forEach(element => {
      this.sheetData.data = _.without(this.sheetData.data, element);
    });

    this.calculateTotalSum();
    this.toast.showSuccess('تم حذف البيانات ');
  }
}

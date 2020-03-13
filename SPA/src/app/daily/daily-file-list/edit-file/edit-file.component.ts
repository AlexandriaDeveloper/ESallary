import { Component, OnInit, ViewChild } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { Subject } from 'rxjs';
import { SharedService } from 'src/app/_services/shared.service';
import { FiletypeService } from 'src/app/_services/filetype.service';
import { NgxSelectComponent, NgxSelectOption } from 'ngx-select-ex';
import { FiletypeModalComponent } from '../../daily-file/filetype-modal/filetype-modal.component';

@Component({
  selector: 'app-edit-file',
  templateUrl: './edit-file.component.html',
  styleUrls: ['./edit-file.component.css']
})
export class EditFileComponent implements OnInit {
  id;
  @ViewChild('fileType', { static: true })
  public ngDepartmentSelect: NgxSelectComponent;
  editFileForm: FormGroup;
  fileData = {
    name: '',
    fileTypeId: '',
    fileNum55: '',
    fileTypeName: '',
    paymentMethod: '',
    collageName: '',
    paymentType: '',
    open: true,
    totalSum: 0
  };
  public onClose = new Subject<any>();
  paymetMethodList: string[];
  collagesNameList: string[];
  paymetTypelist: string[];
  filesType: any = [];

  constructor(
    public modalRef: BsModalRef,
    private fb: FormBuilder,
    private sharedService: SharedService,
    private filesTypeService: FiletypeService,
    private modalService: BsModalService
  ) {}

  ngOnInit() {
    this.paymetMethodList = this.sharedService.paymetMethod();
    this.paymetTypelist = this.sharedService.paymentType();

    this.collagesNameList = this.sharedService.collageName();
    this.filesTypeService.getAllFilesType().subscribe((x: any) => {
      this.filesType = x;
      console.log(x);
    });
    this.onChange();
    console.log(this);
    this.buildForm();
  }
  closeModal() {
    this.modalRef.hide();
  }
  onSubmit() {
    const val = this.editFileForm.value;

    Object.assign(this.fileData, val);
    this.fileData.open = true;

    console.log(this.fileData);
    this.onClose.next(this.fileData);
  }
  buildForm() {
    this.editFileForm = this.fb.group({
      name: [this.fileData.name, Validators.required],
      fileNum55: [this.fileData.fileNum55, Validators.required],
      fileTypeName: [this.fileData.fileTypeName, Validators.required],
      paymentMethod: [this.fileData.paymentMethod, Validators.required],
      collageName: [this.fileData.collageName, Validators.required],
      totalSum: [this.fileData.totalSum, Validators.required],
      paymentType: [this.fileData.paymentType, Validators.required]
    });
  }
  onChange() {
   // this.ngDepartmentSelect.selectionChanges
    //  .subscribe
      // (x: NgxSelectOption[]) => {
      //   console.log(x[0].data.id);

      //   this.fileData.fileTypeId = x[0].data.id;
      // }
    //  ();
  }

  openModal2() {
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
}

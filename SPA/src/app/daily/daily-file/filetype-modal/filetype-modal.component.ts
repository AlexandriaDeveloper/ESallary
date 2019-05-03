import { CustomToastrService } from './../../../_services/toastr.service';
import { FiletypeService } from './../../../_services/filetype.service';
import { Component, OnInit } from '@angular/core';
import { BsModalRef } from 'ngx-bootstrap/modal';
import { forEach } from '@angular/router/src/utils/collection';
import { Subject } from 'rxjs';

@Component({
  selector: 'app-filetype-modal',
  templateUrl: './filetype-modal.component.html',
  styleUrls: ['./filetype-modal.component.css']
})
export class FiletypeModalComponent implements OnInit {
  fileType: any = {};
  public onClose = new Subject<any>();
  constructor(
    public modalRef: BsModalRef,
    private fileTypeService: FiletypeService,
    private toast: CustomToastrService
  ) {}

  ngOnInit() {}
  closeModal() {
    this.modalRef.hide();
  }
  submitForm() {
    this.fileTypeService.postFileType(this.fileType).subscribe(
      x => {
        this.onClose.next(x);
        this.modalRef.hide();
      },
      err => {
        err.error.forEach(element => {
          console.log(element);
          this.toast.showError(element.errorMessage);
        });
      }
    );
  }
}

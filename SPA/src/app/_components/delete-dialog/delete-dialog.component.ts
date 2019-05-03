import { Observable, Subject } from 'rxjs';
import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { BsModalService, BsModalRef } from 'ngx-bootstrap/modal';

@Component({
  selector: 'app-delete-dialog',
  template: `
      <div class="modal-body text-center ">
        <div class="mb-3 pb-3">
          {{ message }} <i class="fa fa-trash fa-lg "></i>
        </div>
        <button
          type="button"
          class="btn btn-default btn-sm"
          (click)="confirm()"
        >
          موافق
        </button>
        <button
          type="button"
          class="btn btn-primary bg-transparent btn-sm"
          (click)="decline()"
        >
          غير موافق
        </button>
      </div>

  `
})
export class DeleteDialogComponent implements OnInit {
  @Input()
  title = '';
  @Input()
  message = '';
  @Output()
  deleteConfirmation: EventEmitter<boolean> = new EventEmitter<boolean>();

  public onClose = new Subject<any>();
  constructor(private modalService: BsModalService) {}

  ngOnInit() {}
  confirm() {
    this.onClose.next(true);
  }
  decline() {
    this.onClose.next(false);
  }
}

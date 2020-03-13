import {
  Component,
  OnInit,
  ElementRef,
  ViewChild,
  AfterViewInit
} from '@angular/core';
import { DailyService } from 'src/app/_services/daily.service';
import { Pagination, PaginatedResult } from 'src/app/_models/Pagination';
import { EmpParams } from 'src/app/_models/employee.model';
import { ActivatedRoute } from '@angular/router';
import { Subscription, from, fromEvent } from 'rxjs';
import { map, pluck, debounceTime, distinctUntilChanged } from 'rxjs/operators';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { EditFileComponent } from './edit-file/edit-file.component';
import { CustomToastrService } from 'src/app/_services/toastr.service';
import { _ } from 'underscore';
import { DeleteDialogComponent } from 'src/app/_components/delete-dialog/delete-dialog.component';
import { GridFlowAnimation } from 'src/app/_animations/grid-flow-animation';

import { SerachModelComponent } from './serach-model/serach-model.component';
@Component({
  selector: 'app-daily-file-list',
  templateUrl: './daily-file-list.component.html',
  styleUrls: ['./daily-file-list.component.css'],
  animations: GridFlowAnimation.animations
})
export class DailyFileListComponent implements OnInit, AfterViewInit {
  @ViewChild('name', { static: false }) nameInputRef: ElementRef;
  ctrlSubscripton: Subscription;

  fileList: any;
  params: EmpParams = new EmpParams();
  pagination: Pagination;
  pageNumber: number;
  pageSize: number;
  maxSize = 10;
  data: any[];
  fileSubscripton: Subscription;
  state = '';
  config = {
    backdrop: true,
    ignoreBackdropClick: true
  };
  modalRef: BsModalRef;
  constructor(
    private modalService: BsModalService,
    private fileService: DailyService,
    private route: ActivatedRoute,
    private toast: CustomToastrService
  ) {}

  ngAfterViewInit(): void {
    this.subscribCtrl(this.nameInputRef.nativeElement);
  }
  subscribCtrl(ctrl) {
    this.ctrlSubscripton = fromEvent(ctrl, 'keyup')
      .pipe(
        pluck('target', 'value'),
        debounceTime(600),
        distinctUntilChanged(),
        map(x => {
          this.loadFiles();
        })
      )
      .subscribe(x => {}, err => {}, () => this.ctrlSubscripton.unsubscribe());
  }
  ngOnInit() {
    this.route.data.subscribe(
      data => {
        console.log(data);
        // this.spinner.show();
        this.fileList = data['emps'].result;
        this.pagination = data['emps'].pagination;
        console.log(this.data);
        //  this.spinner.hide();
        // this.spinner.hide();
      },
      err => {
        // this.spinner.hide();
      },
      () => {
        // this.spinner.hide();
      }
    );
  }
  pageNums(i: number) {
    return new Array(i);
  }
  pageChanged(event: any): void {
    // debugger;
    this.pagination.CurrentPage = event.page;
    //  window.scrollTo(0, 0);
    //this._scrollToService.scrollTo(this.config);
    this.loadFiles();
    // this.loadUsers();
  }

  loadFiles() {
    this.fileList = [];
    this.fileSubscripton = this.fileService
      .getFiles(
        this.pagination.CurrentPage,
        this.pagination.ItemsPerPage,
        this.params
      )
      .pipe(
        map((res: PaginatedResult<any[]>) => {
          // this.spinner.show();

          res.result.forEach(element => {
            this.fileList.push(element);
          });

          this.pagination = res.pagination;
        })
      )
      .subscribe({
        next: function(res) {},
        error: function(err) {},
        complete: function() {
          if (this.empSubscripton != null) {
            this.empSubscripton.unsubscribe();
          }
        }
      });
  }

  openModal(item) {
    console.log(item);
    this.modalRef = this.modalService.show(EditFileComponent, {
      initialState: {
        fileData: item
      },
      ignoreBackdropClick: true,
      keyboard: true
    });

    this.modalRef.content.onClose.subscribe(
      result => {
        console.log(result);

        if (this.state === 'update') {
          const sub = this.fileService.putFile(result).subscribe(
            x => {
              console.log(x);
              item = x;
              this.toast.showSuccess(' تم تحديث الملف ');
            },
            err => {
              console.log(err.error);
              this.toast.showError(err.error[0].errorMessage);
            },
            () => {
              sub.unsubscribe();
            }
          );
        }
        if (this.state === 'new') {
          const sub = this.fileService.getSheetsName(result).subscribe(
            x => {
              console.log(x);

              this.fileList.unshift(x);
              this.toast.showSuccess(' تم إضافة الملف ');
            },
            err => {
              this.toast.showError(err.error[0].errorMessage);
            },
            () => {
              sub.unsubscribe();
            }
          );
        }
        this.modalRef.hide();
      },
      err => {
        console.log(err.error);
        this.toast.showError(err.error[0].errorMessage);
      }
    );
  }
  openSerachModel() {
    this.modalRef = this.modalService.show(SerachModelComponent, {
      initialState: {},
      ignoreBackdropClick: true,
      keyboard: true
    });
  }
  updateFile(model) {
    this.state = 'update';
    this.openModal(model);
  }
  newFile() {
    let item = {};
    this.state = 'new';
    this.openModal(item);
  }
  deleteFile(row) {
    console.log(row);
    //  this.fileList = _.without(this.fileList , row);

    this.modalRef = this.modalService.show(DeleteDialogComponent, {
      initialState: { message: `هل تريد حذف ${row.name}` }
    });
    this.modalRef.content.onClose.subscribe(res => {
      if (res) {
        const deleteSub = this.fileService.deleteFile(row.id).subscribe(
          x => {
            this.fileList = _.without(this.fileList, row);
            this.toast.showSuccess(`تم حذف ملف ${row.name}`);
          },
          err => {
            this.toast.showError(err.error[0].errorMessage);
          },
          () => {
            deleteSub.unsubscribe();
          }
        );
      }

      this.modalRef.hide();
    });
  }
}

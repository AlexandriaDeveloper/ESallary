import { environment } from '../../../environments/environment';
import { EmployeeService } from '../../_services/employee.service';
import {
  Component,
  OnInit,
  EventEmitter,
  Output,
  Input,
  OnDestroy
} from '@angular/core';
import { FileUploader, FileItem, ParsedResponseHeaders } from 'ng2-file-upload';

import { WorkSheet, WorkBook, read , utils} from 'xlsx';
import { Subject, Observable, of, Subscription } from 'rxjs';
import { switchMap, map, catchError } from 'rxjs/operators';


@Component({
  selector: 'app-upload',
  templateUrl: './upload.component.html',
  styleUrls: ['./upload.component.css']
})
export class UploadComponent implements OnInit, OnDestroy {
  baseurl = environment.apiUrl;
  // tslint:disable-next-line:no-input-rename
  @Input('URL')
  urlRes: string;
  URL_API = this.baseurl;
  uploader: FileUploader;
  hasBaseDropZoneOver = false;
  private filesSubject: Subject<File>;
  result = new Array();
  private _uploadedXls: Observable<{ result: string; payload: any }>;
  private subscription: Subscription;
  @Output()
  public uploadedXls: EventEmitter<UploadResult> = new EventEmitter();
  @Output()public upload: EventEmitter<any> = new EventEmitter();
  constructor() {
    this.filesSubject = new Subject();
    this._uploadedXls = this.filesSubject.asObservable().pipe(
      switchMap((file: File) => {
        return new Observable<any>(observer => {
          const reader: FileReader = new FileReader();
          reader.onload = e => {
            observer.next((e.target as any).result);
          };
          reader.readAsBinaryString(file);
          return () => {
            reader.abort();
          };
        });
      }),
      map((value: string) => {
        return read(value, { type: 'binary' });
      }),
      map((wb: WorkBook) => {
        return wb.SheetNames.map((sheetName: string) => {
          const sheet: WorkSheet = wb.Sheets[sheetName];
          return utils.sheet_to_json(sheet, { header: 1 });
        });
      }),
      map((results: Array<any>) => {
        return { result: 'success', payload: results };
      }),
      catchError(e => of({ result: 'failure', payload: e }))
    );
  }
  fileOverBase(e: any): void {
    this.hasBaseDropZoneOver = e;
  }
  initilizeUploader() {
    console.log(this.urlRes);
    this.uploader = new FileUploader({
      url: this.URL_API + this.urlRes,
      authToken: 'Bearer ' + localStorage.getItem('token'),
      // allowedFileType: ['application/x-zip-compresse'],
      allowedMimeType: [
        'application/vnd.ms-excel',
        'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet'
      ],
      autoUpload: false,
      removeAfterUpload: true
    });

    this.uploader.onAfterAddingFile = file => {
      file.withCredentials = false;
    };

    this.uploader.onAfterAddingAll = file => {
      const FileR = new FileReader();
      FileR.onload = function(e: any) {
        // pre-process data
        let binary = '';
        const bytes = e.target.result;
        console.log(bytes);
        const length = bytes.byteLength;
        for (let i = 0; i < length; i++) {
          binary += String.fromCharCode(bytes[i]);
        }
      };

      //  const result = FileR.readAsArrayBuffer(file.formData);
    };
    this.uploader.uploadItem = item => {
      return this.upload.emit(this.uploader);
    };
    this.uploader.onCompleteItem = item => {};
  }

  ngOnInit() {
    this.initilizeUploader();
    this.subscription = this._uploadedXls.subscribe(this.uploadedXls);
  }

  public fileDropped(files: FileList): void {
    for (let i = 0; i < files.length; i++) {
      this.filesSubject.next(files[i]);
    }
  }

  ngOnDestroy(): void {
    this.subscription.unsubscribe();
  }
}
export interface UploadResult {
  result: 'failure' | 'success';
  payload: any;
}

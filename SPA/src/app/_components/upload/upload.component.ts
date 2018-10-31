import { environment } from '../../../environments/environment';
import { EmployeeService } from '../../_services/employee.service';
import { Component, OnInit, EventEmitter, Output, Input } from '@angular/core';
import { FileUploader, FileItem, ParsedResponseHeaders } from 'ng2-file-upload';

import { WorkSheet, WorkBook, read } from 'xlsx';
import { Subject, Observable, of, Subscription } from 'rxjs';
import { switchMap, map, catchError } from 'rxjs/operators';
import { utils } from 'xlsx';

@Component({
  selector: 'app-upload',
  templateUrl: './upload.component.html',
  styleUrls: ['./upload.component.css']
})
export class UploadComponent implements OnInit {
  baseurl = environment.apiUrl;
  @Input()URL: string;
  URL_API = this.baseurl + this.URL;
  uploader: FileUploader;
  hasBaseDropZoneOver = false;
  private filesSubject: Subject<File>;
  result = new Array();
  private _uploadedXls: Observable<{ result: string; payload: any }>;
  private subscription: Subscription;
  @Output()
  public uploadedXls: EventEmitter<UploadResult> = new EventEmitter();
  constructor(private employeeService: EmployeeService) {
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
          console.log(utils.sheet_to_json(sheet, { header: 1 }));
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
    this.uploader = new FileUploader({
      url: this.URL_API,
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
        console.log(e);
        const filename = file.file.name;
        // pre-process data
        let binary = '';
        const bytes = e.target.result;
        console.log(bytes);
        const length = bytes.byteLength;
        for (let i = 0; i < length; i++) {
          binary += String.fromCharCode(bytes[i]);
        }
        const oFile = read(binary, {
          type: 'binary',
          cellDates: true,
          cellStyles: true
        });
      };

      //  const result = FileR.readAsArrayBuffer(file.formData);
    };

    this.uploader.onCompleteItem = item => {
      item.onSuccess = res => {
        this.result.push(item.file);
        console.log(item);
        this.uploader.onSuccessItem = (i, response, status, headers) =>
          this.onSuccessItem(i, response, status, headers);
      };
    };
  }
  onSuccessItem(
    item: FileItem,
    response: string,
    status: number,
    headers: ParsedResponseHeaders
  ): any {
    const data = JSON.parse(response);
    console.log(data);
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
  test() {
    console.log(this.filesSubject.next);
  }
}
export interface UploadResult {
  result: 'failure' | 'success';
  payload: any;
}

import { Router } from '@angular/router';
import { CustomToastrService } from './../../_services/toastr.service';
import { environment } from './../../../environments/environment';
import { EmployeeService } from './../../_services/employee.service';
import { FileUploader, FileItem } from 'ng2-file-upload';
import { Component, OnInit, ChangeDetectorRef } from '@angular/core';

@Component({
  selector: 'app-employee-database',
  templateUrl: './employee-database.component.html',
  styleUrls: ['./employee-database.component.css']
})
export class EmployeeDatabaseComponent implements OnInit {
  URL = environment.apiUrl + 'Employees/UploadEmployees';
  public uploader: FileUploader;
  public hasBaseDropZoneOver = false;
  uploadSuccess = false;

  response: string;
  public fileOverBase(e: any): void {
    this.hasBaseDropZoneOver = e;
    this.uploadSuccess = false;
  }

  constructor(
    private emplyeeDBService: EmployeeService,
    private changeDetector: ChangeDetectorRef,
    private toast: CustomToastrService,
    private route: Router
  ) {}

  ngOnInit() {
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
      console.log(file);
    };
    this.uploader.onBuildItemForm = (fileItem: any, form: any) => {
      // add custom Propety
      form.append('name', 'mohamed');
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
      res => {
        this.response = res;
        this.toast.showSuccess('تم رفع الملف بنجاح');
      },
      err => {
        this.toast.showError(err.error);
      },
      () => {
        this.route.navigate(['/home']);
      }
    );
  }
}

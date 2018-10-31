import { Component, OnInit } from '@angular/core';
import { UploadResult } from 'src/app/_components/upload/upload.component';
import { BehaviorSubject } from 'rxjs';

@Component({
  selector: 'app-employee-database',
  templateUrl: './employee-database.component.html',
  styleUrls: ['./employee-database.component.css']
})
export class EmployeeDatabaseComponent implements OnInit {
  public uploaderContent: BehaviorSubject<string> = new BehaviorSubject(
    'please drop file in'
  );
  header;
  items;
  constructor() {}

  ngOnInit() {}
  public xlsxUploaded(result: UploadResult) {
    this.items = result.payload[0];
    this.header = this.items.shift();
    this.uploaderContent.next(JSON.stringify(result));
  }
}

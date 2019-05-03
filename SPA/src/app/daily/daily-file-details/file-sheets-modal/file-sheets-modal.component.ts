import { DailyService } from './../../../_services/daily.service';
import { Component, OnInit } from '@angular/core';
import { BsModalRef } from 'ngx-bootstrap/modal';
import { Subject } from 'rxjs';

@Component({
  selector: 'app-file-sheets-modal',
  templateUrl: './file-sheets-modal.component.html',
  styleUrls: ['./file-sheets-modal.component.css']
})
export class FileSheetsModalComponent implements OnInit {
  selectedSheets = [];
  sheets = [];
  res: any;
  defaultPaymentMethod:any;
  public onClose = new Subject<any>();
  constructor(public modalRef: BsModalRef, private fileService: DailyService) {}

  ngOnInit() {
    this.sheets = this.res.sheets;
    console.log(this.sheets);
    console.log(this.res);
    console.log(this.defaultPaymentMethod);
  }
  addSheetName(sheet) {
    if (sheet.target.checked) {
      this.selectedSheets.push(sheet.target.value);
    } else {
      this.selectedSheets = this.selectedSheets.filter(
        x => x !== sheet.target.value
      );
    }
    console.log(this.selectedSheets);
  }
  getSheetData() {
    this.res.selectedSheets = this.selectedSheets;
    console.log(this.res);
    this.fileService.readSheets(this.res).subscribe(x => this.onClose.next(x));
  }
}

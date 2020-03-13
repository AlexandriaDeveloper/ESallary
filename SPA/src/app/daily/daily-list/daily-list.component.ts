import { Component, OnInit } from '@angular/core';
import { DailiesService } from 'src/app/_services/dailies.service';
import { Pagination, PaginatedResult } from 'src/app/_models/Pagination';
import { Subscription } from 'rxjs';
import { map } from 'rxjs/operators';
import { EmpParams, EmployeeList } from 'src/app/_models/employee.model';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-daily-list',
  templateUrl: './daily-list.component.html',
  styleUrls: ['./daily-list.component.css']
})
export class DailyListComponent implements OnInit {
  dailiesList: any = [];
  pagination: Pagination;
  pageNumber: number;
  pageSize: number;
  maxSize = 10;

  empSubscripton: Subscription;
  empParams: EmpParams = new EmpParams();

  bsValue = new Date();
  bsRangeValue: Date[];
  minDate = new Date();
  maxDate = new Date();
  constructor(
    private dailyService: DailiesService,
    private route: ActivatedRoute
  ) {}

  ngOnInit() {
    this.minDate.setDate(this.maxDate.getDate() - 30);
    this.maxDate.setDate(this.maxDate.getDate());
    this.bsRangeValue = [this.minDate, this.maxDate];

    this.route.data.subscribe(
      data => {
        // this.spinner.show();
        this.dailiesList = data['emps'].result;
        this.pagination = data['emps'].pagination;
        console.log(data['emps']);
        //  this.spinner.hide();
        //  this.spinner.hide();
      },
      err => {
        //  this.spinner.hide();
      },
      () => {
        //  this.spinner.hide();
      }
    );
  }

  // loadUsers() {
  //   this.data = [];
  //   this.empSubscripton = this.dailyService
  //     .getFile(
  //       this.pagination.CurrentPage,
  //       this.pagination.ItemsPerPage,
  //       this.empParams
  //     )
  //     .pipe(
  //       map((res: PaginatedResult<EmployeeList[]>) => {
  //         // this.spinner.show();
  //         this.data = res.result;
  //         this.pagination = res.pagination;
  //       })
  //     )
  //     .subscribe({
  //       next: function(res) {},
  //       error: function(err) {},
  //       complete: function() {
  //         if (this.empSubscripton != null) {
  //           this.empSubscripton.unsubscribe();
  //         }
  //       }
  //     });
  // }
  pageChanged(event: any): void {
    this.pagination.CurrentPage = event.page;
    //  window.scrollTo(0, 0);
    // this._scrollToService.scrollTo(this.config);
    // this.loadUsers();
    // this.loadUsers();
  }
  toggle() {
    console.log(this.bsRangeValue[0]);
    console.log(this.bsRangeValue[1]);
  }
}

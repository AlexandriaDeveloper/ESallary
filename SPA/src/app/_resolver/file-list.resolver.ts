import { PaginatedResult } from '../_models/Pagination';
import { Injectable } from '@angular/core';
import { Resolve, ActivatedRouteSnapshot } from '@angular/router';
import { Observable } from 'rxjs';
import { DailyService } from '../_services/daily.service';
@Injectable({
  providedIn: 'root'
})
export class FileResolver implements Resolve<PaginatedResult<any[]>> {
  pageNumber = 1;
  pageSize = 30;
  constructor(private fileService: DailyService) { }
  resolve(route: ActivatedRouteSnapshot): Observable<PaginatedResult<any[]>> {
    console.log(route);
    return this.fileService.getFiles(this.pageNumber, this.pageSize).pipe();
  }
}

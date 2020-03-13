import { PaginatedResult } from '../_models/Pagination';
import { Injectable } from '@angular/core';
import { Resolve, ActivatedRouteSnapshot } from '@angular/router';
import { Observable } from 'rxjs';

import { DailiesService } from '../_services/dailies.service';
@Injectable({
  providedIn: 'root'
})
export class DailyResolver implements Resolve<PaginatedResult<any[]>> {
  pageNumber = 1;
  pageSize = 30;
  constructor(private fileService: DailiesService) {}
  resolve(route: ActivatedRouteSnapshot): Observable<any> {
    console.log(route);
    return this.fileService.getFile(this.pageNumber, this.pageSize).pipe();
  }
}

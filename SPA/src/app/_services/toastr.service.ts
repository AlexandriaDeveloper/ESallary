import { Injectable, OnInit, ViewChild } from '@angular/core';

import { ToastrService, ToastContainerDirective } from 'ngx-toastr';

@Injectable({
  providedIn: 'root'
})
export class CustomToastrService implements OnInit {
  @ViewChild(ToastContainerDirective) toastContainer: ToastContainerDirective;
  constructor(private toastrService: ToastrService) {}
  // confirm(message: string, okCallback: () => any) {
  //   this.toastrService.show()
  //   if (e) {
  //     okCallback();
  //   } else {
  //   }
  // }
  ngOnInit() {
    this.toastrService.overlayContainer = this.toastContainer;
  }
  showSuccess(msg) {
    this.toastrService.success(msg, ' تم بنجاح');
  }
  showError(msg) {
    this.toastrService.error(msg, ' خطأ ');
  }
  showWarrning(msg) {
    this.toastrService.warning(msg, ' تنبيه ');
  }
  showInfo(msg) {
    this.toastrService.info(msg, ' بيان ');
  }
}

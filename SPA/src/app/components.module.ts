import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CollapseModule } from 'ngx-bootstrap/collapse';
import { CarouselModule } from 'ngx-bootstrap/carousel';
import { ModalModule } from 'ngx-bootstrap/modal';
import { TooltipModule } from 'ngx-bootstrap/tooltip';
import { PaginationModule } from 'ngx-bootstrap/pagination';
import { PopoverModule } from 'ngx-bootstrap/popover';
import { NgxSelectModule, INgxSelectOptions } from 'ngx-select-ex';
import { NgxSpinnerModule } from 'ngx-spinner';

import { ScrollToModule } from '@nicky-lenaers/ngx-scroll-to';

import { ToastrModule } from 'ngx-toastr';
import { BankService } from './_services/bank.service';
import { ClickOutsideDirective } from './_directives/clickOutside.directive';
import { NullDefaultValueDirective } from './_directives/nullDefaultValue.directive';
import { OnlyNumberDirective } from './_directives/onlyNumber.directive';

export const CustomSelectOptions: INgxSelectOptions = {
  // Check the interface for more options
  optionValueField: 'id',
  optionTextField: 'name',
  keepSelectedItems: true,
  placeholder: 'أختار '
};

export const ToastConfig = {
  closeButton: true,

  positionClass: 'toast-bottom-right',
  preventDuplicates: true,
  easing: 'ease-in',
  timeOut: 5000,
  extendedTimeOut: 2000
};
@NgModule({
  imports: [
    CommonModule,
    NgxSelectModule,
    PopoverModule,
    NgxSelectModule.forRoot(CustomSelectOptions),
    NgxSpinnerModule,
    PaginationModule.forRoot(),
    ScrollToModule.forRoot(),
    ToastrModule.forRoot(ToastConfig),
    CollapseModule.forRoot(),
    CarouselModule.forRoot(),
    ModalModule.forRoot(),
    TooltipModule.forRoot()
  ],
  declarations: [
    OnlyNumberDirective,
    ClickOutsideDirective,
    NullDefaultValueDirective],
  providers: [],
  exports: [
    CommonModule,
    // NgxSelectModule,
    PopoverModule,
    NgxSelectModule,
    NgxSpinnerModule,
    PaginationModule,
    ScrollToModule,
    ToastrModule,
    CollapseModule,
    CarouselModule,
    ModalModule,
    TooltipModule,
    OnlyNumberDirective,
    ClickOutsideDirective,
    NullDefaultValueDirective
  ]
})
export class ComponentsModule {}

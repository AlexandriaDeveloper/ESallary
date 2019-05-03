/* tslint:disable:no-unused-variable */
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { DebugElement } from '@angular/core';

import { EmployeeDetalisAddOrderBankModelComponent } from './employee-detalis-add-order-bank-model.component';

describe('EmployeeDetalisAddOrderBankModelComponent', () => {
  let component: EmployeeDetalisAddOrderBankModelComponent;
  let fixture: ComponentFixture<EmployeeDetalisAddOrderBankModelComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ EmployeeDetalisAddOrderBankModelComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(EmployeeDetalisAddOrderBankModelComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

/* tslint:disable:no-unused-variable */
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { DebugElement } from '@angular/core';

import { EmployeeDetailsAddOrderBranchComponent } from './employee-details-add-order-branch.component';

describe('EmployeeDetailsAddOrderBranchComponent', () => {
  let component: EmployeeDetailsAddOrderBranchComponent;
  let fixture: ComponentFixture<EmployeeDetailsAddOrderBranchComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ EmployeeDetailsAddOrderBranchComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(EmployeeDetailsAddOrderBranchComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

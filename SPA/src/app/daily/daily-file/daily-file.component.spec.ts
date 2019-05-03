/* tslint:disable:no-unused-variable */
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { DebugElement } from '@angular/core';

import { DailyFileComponent } from './daily-file.component';

describe('DailyFileComponent', () => {
  let component: DailyFileComponent;
  let fixture: ComponentFixture<DailyFileComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DailyFileComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DailyFileComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

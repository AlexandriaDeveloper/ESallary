/* tslint:disable:no-unused-variable */
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { DebugElement } from '@angular/core';

import { InputdebounceComponent } from './inputdebounce.component';

describe('InputdebounceComponent', () => {
  let component: InputdebounceComponent;
  let fixture: ComponentFixture<InputdebounceComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ InputdebounceComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(InputdebounceComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

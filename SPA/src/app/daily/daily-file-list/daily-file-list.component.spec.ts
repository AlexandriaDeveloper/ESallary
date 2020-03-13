import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DailyFileListComponent } from './daily-file-list.component';

describe('DailyFileListComponent', () => {
  let component: DailyFileListComponent;
  let fixture: ComponentFixture<DailyFileListComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DailyFileListComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DailyFileListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

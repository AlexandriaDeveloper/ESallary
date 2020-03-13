import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SerachModelComponent } from './serach-model.component';

describe('SerachModelComponent', () => {
  let component: SerachModelComponent;
  let fixture: ComponentFixture<SerachModelComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SerachModelComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SerachModelComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

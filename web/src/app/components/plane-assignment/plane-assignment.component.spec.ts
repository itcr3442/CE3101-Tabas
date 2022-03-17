import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PlaneAssignmentComponent } from './plane-assignment.component';

describe('PlaneAssignmentComponent', () => {
  let component: PlaneAssignmentComponent;
  let fixture: ComponentFixture<PlaneAssignmentComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ PlaneAssignmentComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(PlaneAssignmentComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

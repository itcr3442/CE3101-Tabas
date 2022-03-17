import { ComponentFixture, TestBed } from '@angular/core/testing';

import { BagCartAssignmentComponent } from './bag-cart-assignment.component';

describe('BagCartAssignmentComponent', () => {
  let component: BagCartAssignmentComponent;
  let fixture: ComponentFixture<BagCartAssignmentComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ BagCartAssignmentComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(BagCartAssignmentComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

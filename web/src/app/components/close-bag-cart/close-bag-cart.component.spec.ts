import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CloseBagCartComponent } from './close-bag-cart.component';

describe('CloseBagCartComponent', () => {
  let component: CloseBagCartComponent;
  let fixture: ComponentFixture<CloseBagCartComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CloseBagCartComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(CloseBagCartComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

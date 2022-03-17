import { ComponentFixture, TestBed } from '@angular/core/testing';

import { BagCartCreationComponent } from './bag-cart-creation.component';

describe('BagCartCreationComponent', () => {
  let component: BagCartCreationComponent;
  let fixture: ComponentFixture<BagCartCreationComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ BagCartCreationComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(BagCartCreationComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

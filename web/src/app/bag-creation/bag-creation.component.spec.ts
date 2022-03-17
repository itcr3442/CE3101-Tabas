import { ComponentFixture, TestBed } from '@angular/core/testing';

import { BagCreationComponent } from './bag-creation.component';

describe('BagCreationComponent', () => {
  let component: BagCreationComponent;
  let fixture: ComponentFixture<BagCreationComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ BagCreationComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(BagCreationComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

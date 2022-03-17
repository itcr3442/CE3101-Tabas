import { ComponentFixture, TestBed } from '@angular/core/testing';

import { BagsListComponent } from './bags-list.component';

describe('BagsListComponent', () => {
  let component: BagsListComponent;
  let fixture: ComponentFixture<BagsListComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ BagsListComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(BagsListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

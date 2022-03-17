import { ComponentFixture, TestBed } from '@angular/core/testing';

import { BagcartsListComponent } from './bagcarts-list.component';

describe('BagcartsListComponent', () => {
  let component: BagcartsListComponent;
  let fixture: ComponentFixture<BagcartsListComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ BagcartsListComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(BagcartsListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

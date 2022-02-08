import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TransformRemoveColumnsComponent } from './transform-remove-columns.component';

describe('TransformRemoveColumnsComponent', () => {
  let component: TransformRemoveColumnsComponent;
  let fixture: ComponentFixture<TransformRemoveColumnsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ TransformRemoveColumnsComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(TransformRemoveColumnsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

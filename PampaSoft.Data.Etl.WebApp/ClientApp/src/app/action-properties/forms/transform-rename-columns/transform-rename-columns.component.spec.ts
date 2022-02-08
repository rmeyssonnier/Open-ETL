import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TransformRenameColumnsComponent } from './transform-rename-columns.component';

describe('TransformRenameColumnsComponent', () => {
  let component: TransformRenameColumnsComponent;
  let fixture: ComponentFixture<TransformRenameColumnsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ TransformRenameColumnsComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(TransformRenameColumnsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

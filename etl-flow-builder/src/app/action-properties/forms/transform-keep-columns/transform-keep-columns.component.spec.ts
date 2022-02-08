import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TransformKeepColumnsComponent } from './transform-keep-columns.component';

describe('TransformKeepColumnsComponent', () => {
  let component: TransformKeepColumnsComponent;
  let fixture: ComponentFixture<TransformKeepColumnsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ TransformKeepColumnsComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(TransformKeepColumnsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

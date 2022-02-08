import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ExtractMinioActionComponent } from './extract-minio-action.component';

describe('LoadMinioActionComponent', () => {
  let component: ExtractMinioActionComponent;
  let fixture: ComponentFixture<ExtractMinioActionComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ExtractMinioActionComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ExtractMinioActionComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

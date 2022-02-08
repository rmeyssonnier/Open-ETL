import { ComponentFixture, TestBed } from '@angular/core/testing';

import { LoadMinioActionComponent } from './load-minio-action.component';

describe('LoadMinioActionComponent', () => {
  let component: LoadMinioActionComponent;
  let fixture: ComponentFixture<LoadMinioActionComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ LoadMinioActionComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(LoadMinioActionComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ExtractHttpActionComponent } from './extract-http-action.component';

describe('LoadHttpActionComponent', () => {
  let component: ExtractHttpActionComponent;
  let fixture: ComponentFixture<ExtractHttpActionComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ExtractHttpActionComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ExtractHttpActionComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

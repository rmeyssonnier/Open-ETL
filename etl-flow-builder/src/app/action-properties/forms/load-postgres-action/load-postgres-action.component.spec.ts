import { ComponentFixture, TestBed } from '@angular/core/testing';

import { LoadPostgresActionComponent } from './load-postgres-action.component';

describe('LoadPostgresActionComponent', () => {
  let component: LoadPostgresActionComponent;
  let fixture: ComponentFixture<LoadPostgresActionComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ LoadPostgresActionComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(LoadPostgresActionComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

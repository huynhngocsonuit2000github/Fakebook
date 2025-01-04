import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SetsComponent } from './sets.component';

describe('SetsComponent', () => {
  let component: SetsComponent;
  let fixture: ComponentFixture<SetsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [SetsComponent]
    })
      .compileComponents();

    fixture = TestBed.createComponent(SetsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

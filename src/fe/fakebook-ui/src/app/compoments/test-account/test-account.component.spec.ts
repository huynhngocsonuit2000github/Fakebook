import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TestAccountComponent } from './test-account.component';

describe('TestAccountComponent', () => {
  let component: TestAccountComponent;
  let fixture: ComponentFixture<TestAccountComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [TestAccountComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(TestAccountComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

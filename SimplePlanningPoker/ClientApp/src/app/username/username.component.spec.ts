import { ComponentFixture, TestBed } from '@angular/core/testing';

import { UsernameComponent } from './username.component';
import { ActivatedRoute, Router, provideRouter } from '@angular/router';
import { FormsModule } from '@angular/forms';

describe('UsernameComponent', () => {
  let component: UsernameComponent;
  let fixture: ComponentFixture<UsernameComponent>;
  let activatedRouteStub: Partial<ActivatedRoute>;
  let routerStub: Partial<Router>;
  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [FormsModule],
      declarations: [UsernameComponent],
      providers: [provideRouter([{ path: 'username', component: UsernameComponent }])]
    });
    fixture = TestBed.createComponent(UsernameComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

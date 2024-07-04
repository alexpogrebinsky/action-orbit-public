import { TestBed, async, ComponentFixture } from '@angular/core/testing';
import { RouterTestingModule } from '@angular/router/testing';
import { of } from 'rxjs';
import { AppComponent } from './app.component';
import { AuthService } from './services/auth-service/auth.service';
import { Component } from '@angular/core';
import { MatSidenavModule } from '@angular/material/sidenav';
import { NoopAnimationsModule } from '@angular/platform-browser/animations';

@Component({ selector: 'app-profile-dropdown-widget', template: '' })
class ProfileDropdownWidgetComponentStub { }
// Stub HeaderComponent
@Component({ selector: 'app-header', template: '' })
class HeaderComponentStub { }

describe('AppComponent', () => {
  let component: AppComponent;
  let fixture: ComponentFixture<AppComponent>;
  let authService: AuthService;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      imports: [RouterTestingModule, MatSidenavModule, NoopAnimationsModule],
      declarations: [AppComponent, HeaderComponentStub, ProfileDropdownWidgetComponentStub], // Include ProfileDropdownWidgetComponentStub here
      providers: [
        { provide: AuthService, useValue: { isAuthenticated: () => of(true), logout: () => of(null) } }
      ]
    }).compileComponents();

    fixture = TestBed.createComponent(AppComponent);
    component = fixture.componentInstance;
    authService = TestBed.inject(AuthService);
  }));

  it('should create the app', () => {
    expect(component).toBeTruthy();
  });

  it('should navigate to login page on logout', () => {
    spyOn(authService, 'logout').and.returnValue(of(null));
    const navigateSpy = spyOn((<any>component).router, 'navigate');
    component.logout();
    expect(navigateSpy).toHaveBeenCalledWith(['/login']);
  });
});

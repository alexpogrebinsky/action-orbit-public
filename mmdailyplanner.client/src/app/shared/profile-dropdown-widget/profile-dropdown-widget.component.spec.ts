import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ProfileDropdownWidgetComponent } from './profile-dropdown-widget.component';

describe('ProfileDropdownWidgetComponent', () => {
  let component: ProfileDropdownWidgetComponent;
  let fixture: ComponentFixture<ProfileDropdownWidgetComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ProfileDropdownWidgetComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(ProfileDropdownWidgetComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

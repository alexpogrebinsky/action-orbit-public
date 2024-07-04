import { Component, Input, OnInit, AfterViewInit, AfterViewChecked, ChangeDetectorRef } from '@angular/core';
import { AuthService } from '../../services/auth-service/auth.service';
import { Router } from '@angular/router';
import { MatSidenav } from '@angular/material/sidenav';
import { Observable } from 'rxjs';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.css']
})
export class HeaderComponent implements OnInit, AfterViewInit, AfterViewChecked {
  private _sidenav: MatSidenav | undefined;
  private _isAuthenticated: boolean = false;

  @Input()
  set sidenav(sidenav: MatSidenav | undefined) {
    this._sidenav = sidenav;
    console.log('sidenav set:', this._sidenav);
    if (this._isAuthenticated) {
      this.scheduleToggleSidenav();
    }
  }

  get sidenav(): MatSidenav | undefined {
    return this._sidenav;
  }

  isAuthenticated: Observable<boolean> | undefined;

  constructor(
    public authService: AuthService,
    private router: Router,
    private cd: ChangeDetectorRef) { }

  ngOnInit(): void {
  }

  ngAfterViewInit(): void {
    this.isAuthenticated = this.authService.getAuthState();
    this.isAuthenticated.subscribe((authState) => {
      this._isAuthenticated = authState;
      console.log('auth state changed:', this._isAuthenticated);
      if (this._sidenav) {
        this.scheduleToggleSidenav();
      }
    });

    if (this._sidenav && this._isAuthenticated) {
      this.scheduleToggleSidenav();
    }
  }

  ngAfterViewChecked(): void {
    this.cd.detectChanges();
  }

  scheduleToggleSidenav(): void {
    setTimeout(() => {
      this.toggleSidenav();
    });
  }

  toggleSidenav(): void {
    console.log('toggling sidenav');
    if (this._sidenav) {
      this._sidenav.toggle();
    }
  }
}

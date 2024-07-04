import { Component, OnInit, ViewChild, AfterViewInit } from '@angular/core';
import { AuthService } from './services/auth-service/auth.service';
import { Router } from '@angular/router';
import { NAME_CONFIG } from '../config';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  title = NAME_CONFIG.appTitle;
  opened: boolean = true;
  events: string[] = [];

  constructor(public authService: AuthService, private router: Router) { }

  ngOnInit() {
    this.authService.isAuthenticated().subscribe(isAuthenticated => {
      if (isAuthenticated) {
        this.router.navigate(['/tasks']);
      } else {
        this.logout();
      }
    });
  }

  logout() {
    this.authService.logout().subscribe(() => {
      this.router.navigate(['/login']);
    });
  }
}

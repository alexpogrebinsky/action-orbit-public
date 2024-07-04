import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { AuthService } from '../../services/auth-service/auth.service'
import { TaskService } from '../../services/task-service/task.service';
import { User } from '../../models/user.models';
import { Router } from '@angular/router';
import { Observable } from 'rxjs';
import { Subscription } from 'rxjs';
import { UserProfileDto } from '../../dtos/user-profile.dto';

@Component({
  selector: 'app-profile-dropdown-widget',
  templateUrl: './profile-dropdown-widget.component.html',
  styleUrls: ['./profile-dropdown-widget.component.css']
})
export class ProfileDropdownWidgetComponent implements OnInit {
  user!: UserProfileDto | undefined;
  loading = false;
  isAuthenticated: boolean | undefined; 
  authSubscription: Subscription | undefined;

  constructor(
    private http: HttpClient,
    private authService: AuthService,
    private taskService: TaskService,
    private router: Router
  ) {
  }


  async ngOnInit(): Promise<void> {
    this.authSubscription = this.authService.getAuthState().subscribe(async (isAuthenticated) => {
      this.isAuthenticated = isAuthenticated;
      if (isAuthenticated) {
        await this.getUserData();
      }
    });

  }

  ngOnDestroy(): void {
    if (this.authSubscription) {
      this.authSubscription.unsubscribe(); 
    }
  }

  logout() {
    this.authService.logout().subscribe(result => {
      console.log(result)
      this.user = undefined;
});
    this.router.navigate(['/login']);

  }

  async getUserData(): Promise<void> {
    this.authService.getUser().subscribe(data => {
      this.user = data;
    });
  }
    }

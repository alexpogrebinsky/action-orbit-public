import { Component, OnInit } from '@angular/core';
import { AuthService } from '../../services/auth-service/auth.service';
import { UserProfileDto } from '../../dtos/user-profile.dto';

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.css']
})
export class ProfileComponent implements OnInit {
  user!: UserProfileDto;

  constructor(private authService: AuthService) { }

  async ngOnInit(): Promise<void> {
    await this.getUserData();
  }

  async getUserData(): Promise<void> {
    this.authService.getUser().subscribe(data => {
      this.user = data;
    });
  }
}

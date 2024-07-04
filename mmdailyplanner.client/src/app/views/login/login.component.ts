import { Component } from '@angular/core';
import { AuthService } from '../../services/auth-service/auth.service';
import { Router } from '@angular/router';
import { ERROR_MESSAGES } from '../../../messages';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent {
  username: string = '';
  password: string = '';
  loginError: string = '';

  constructor(private authService: AuthService, private router: Router) { }

  async login() {
    await this.authService.login(this.username, this.password).subscribe(
      () => {
        this.router.navigate(['/tasks']);
      },
      (error) => {
        this.loginError = ERROR_MESSAGES.loginError;
      }
    );
  }
}

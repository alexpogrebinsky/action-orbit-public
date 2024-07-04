
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, Observable, Subject } from 'rxjs';
import { map, tap } from 'rxjs/operators';
import { API_CONFIG } from '../../../config';
import { UserProfileDto } from '../../dtos/user-profile.dto';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  userLoggedOut = new Subject<void>();
  private authState = new BehaviorSubject<boolean>(false); 
  user!: UserProfileDto;

  constructor(private http: HttpClient) {
    this.checkAuthState(); 
}

  login(username: string, password: string): Observable<any> {
    return this.http.post(API_CONFIG.login, { username, password }, { withCredentials: true }).pipe(
      tap((response: any) => {
        console.log('Login successful:', response.message);
        this.authState.next(true); 
      })
    );
  }

  register(formData: FormData): Observable<any> {
    return this.http.post(API_CONFIG.register, formData);
  }

  isAuthenticated(): Observable<boolean> {
    return this.http.get<boolean>(API_CONFIG.isAuthenticated, { withCredentials: true });
  }

  getUser(): Observable<UserProfileDto> {
    return this.http.get<UserProfileDto>(API_CONFIG.getUser, { withCredentials: true }).pipe(
      map(data => {
        let base64String = '';
        if (Array.isArray(data.profileImage)) {
          base64String = btoa(String.fromCharCode.apply(null, Array.from(new Uint8Array(data.profileImage))));
        } else if (typeof data.profileImage === 'string') {
          base64String = data.profileImage;
        }
        data.profileImage = 'data:image/png;base64,' + base64String;
        return data;
      }),
      tap(user => this.user = user)
    );
  }

  getCurrentUserId(): Observable<number> {
    return this.http.get<number>(API_CONFIG.getCurrentUserId, { withCredentials: true });
  }

  logout(): Observable<any> {
    return this.http.post(API_CONFIG.logout, {}, { withCredentials: true }).pipe(
      tap(() => {
        this.userLoggedOut.next();
        console.log('Logout successful');
        this.authState.next(false); 
      })
    );
  }

  getAuthState(): Observable<boolean> { 
    return this.authState.asObservable();
  }

  private checkAuthState(): void {
    this.isAuthenticated().subscribe(isAuthenticated => {
      this.authState.next(isAuthenticated);
    });
  }
  }


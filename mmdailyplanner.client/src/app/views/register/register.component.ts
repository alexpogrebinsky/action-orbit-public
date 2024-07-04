import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { AuthService } from '../../services/auth-service/auth.service';
import { Router } from '@angular/router';
import { ERROR_MESSAGES } from '../../../messages';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
  registerForm: FormGroup;
  registrationError: string = '';
  profileImage: File | null = null;
  fileName: string = '';

  constructor(
    private fb: FormBuilder,
    private authService: AuthService,
    private router: Router) {
    this.registerForm = this.fb.group({
      username: ['', [Validators.required]],
      firstName: ['', [Validators.required]],
      lastName: ['', [Validators.required]],
      phoneNumber: ['', [Validators.required, Validators.pattern(/^[0-9]{10}$/)]],
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required, Validators.minLength(6)]],
      terms: [false, Validators.requiredTrue] 
    });
  }

  ngOnInit(): void { }

  onFileSelected(event: Event): void {
    const input = event.target as HTMLInputElement;
    if (input.files && input.files.length > 0) {
      this.profileImage = input.files[0];
      this.fileName = input.files[0].name;
    }
  }

  private appendFormData(formData: FormData): void {
    const formValues = this.registerForm.value;
    formData.append('username', formValues.username);
    formData.append('firstName', formValues.firstName);
    formData.append('lastName', formValues.lastName);
    formData.append('phoneNumber', formValues.phoneNumber);
    formData.append('email', formValues.email);
    formData.append('password', formValues.password);
  }

  private handleRegisterSuccess(): void {
    this.router.navigate(['/login']);
  }

  private handleRegisterError(error: any): void {
    this.registrationError = ERROR_MESSAGES.registrationError;
    console.error(ERROR_MESSAGES.registrationError, error);
  }

  register(): void {
    if (this.registerForm.invalid) {
      this.registrationError = "Please fill out the form correctly.";
      return;
    }

    const formData = new FormData();
    this.appendFormData(formData);

    if (this.profileImage) {
      this.readProfileImage(formData);
    } else {
      this.sendRegisterRequest(formData);
    }
  }

  private readProfileImage(formData: FormData): void {
    if (!this.profileImage) return;

    const reader = new FileReader();
    reader.onload = () => {
      const arrayBuffer = reader.result as ArrayBuffer;
      const byteArray = new Uint8Array(arrayBuffer);
      const blob = new Blob([byteArray], { type: this.profileImage!.type });
      formData.append('ProfileImage', blob, this.profileImage!.name);
      this.sendRegisterRequest(formData);
    };
    reader.readAsArrayBuffer(this.profileImage);
  }

  private sendRegisterRequest(formData: FormData): void {
    this.authService.register(formData).subscribe(
      () => this.handleRegisterSuccess(),
      (error) => this.handleRegisterError(error)
    );
  }
}

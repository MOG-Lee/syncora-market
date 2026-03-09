import { Component, inject, signal } from '@angular/core';
import { AbstractControl, FormBuilder, ReactiveFormsModule, ValidationErrors, ValidatorFn, Validators } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { AuthService } from '../../../../core/services/auth/auth.service';
import { RegisterRequest } from '../../../../core/models/auth.model';

@Component({
  selector: 'app-register',
  imports: [ReactiveFormsModule, RouterLink],
  templateUrl: './register.html',
  styleUrl: './register.scss',
})
export class Register {
  private fb = inject(FormBuilder);
  private router = inject(Router);
  private authService = inject(AuthService);

  isLoading = signal(false);
  errorMessage = signal('');

  // Custom Validator Logic
  passwordMatchValidator: ValidatorFn = (control: AbstractControl): ValidationErrors | null => {
    const pass = control.get('password');
    const confirm = control.get('confirmPassword');
    return pass && confirm && pass.value !== confirm.value ? { mismatch: true } : null;
  };

  // Form with Custom Validator 
  registerForm = this.fb.group({
    name: ['', [Validators.required, Validators.minLength(2)]],
    email: ['', [Validators.required, Validators.email]],
    password: ['', [Validators.required, Validators.minLength(6)]],
    confirmPassword: ['', Validators.required]
  }, { validators: this.passwordMatchValidator });

  onSubmit() {
    if (this.registerForm.invalid)
      return;
    this.isLoading.set(true);
    this.errorMessage.set('');

    const fullName = this.registerForm.value.name!.trim().split(' ');
    const firstName = fullName[0];
    const lastName = fullName.length > 1 ? fullName.slice(1).join(' ') : 'User'; // Fallback if they only type one name

    const request: RegisterRequest = {
      firstName: firstName,
      lastName: lastName,
      email: this.registerForm.value.email!,
      password: this.registerForm.value.password!
    };

    this.authService.register(request).subscribe({
      next: () => {
        this.isLoading.set(false);
        this.router.navigate(['/login']);
      },
      error: (err) => {
        this.isLoading.set(false);
        if (err.status === 400 && typeof err.error === 'string') {
          this.errorMessage.set(err.error);
        } else {
          this.errorMessage.set('An error occurred during registration. Please try again.');
        }
      }
    });
  }
}

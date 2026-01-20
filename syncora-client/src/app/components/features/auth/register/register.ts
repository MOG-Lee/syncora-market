import { Component, inject, signal } from '@angular/core';
import { AbstractControl, FormBuilder, ReactiveFormsModule, ValidationErrors, ValidatorFn, Validators } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';

@Component({
  selector: 'app-register',
  imports: [ReactiveFormsModule, RouterLink],
  templateUrl: './register.html',
  styleUrl: './register.scss',
})
export class Register {
  private fb = inject(FormBuilder);
  private router = inject(Router);

  isLoading = signal(false);

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
    if (this.registerForm.invalid) return;
    this.isLoading.set(true);

    // Simulate API
    setTimeout(() => {
      this.router.navigate(['/login']); // Redirect to login after success
      this.isLoading.set(false);
    }, 1500);
  }
}

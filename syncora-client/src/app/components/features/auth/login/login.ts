import { Component, inject, signal } from '@angular/core';
import { ReactiveFormsModule, FormBuilder, Validators } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';

@Component({
  selector: 'app-login',
  imports: [ReactiveFormsModule, RouterLink],
  templateUrl: './login.html',
  styleUrl: './login.scss',
})
export class Login {
  private fb = inject(FormBuilder);
  private router = inject(Router);

  isLoading = signal(false);
  errorMessage = signal('');

  // The Form Model
  loginForm = this.fb.group({
    email: ['', [Validators.required, Validators.email]],
    password: ['', [Validators.required, Validators.minLength(6)]]
  });

  onSubmit() {
    if (this.loginForm.invalid)
      return;

    this.isLoading.set(true);
    this.errorMessage.set('');

    // SIMULATION: Simulate API call
    setTimeout(() => {

      // Object Destructuring
      const { email, password } = this.loginForm.value;
      // if we do not use object destructuring, we need sth like this
      //const email = this.loginForm.value.email;        
      //const password = this.loginForm.value.password;  

      // Mock Success Logic
      if (email === 'vendor@test.com') {
        // In real app: AuthService.login(token)
        this.router.navigate(['/vendor/dashboard']);
      } else {
        this.router.navigate(['/']); // Go to Home
      }

      this.isLoading.set(false);
    }, 1500);
  }



}

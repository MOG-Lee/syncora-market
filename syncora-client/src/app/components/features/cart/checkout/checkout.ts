import { Component, inject, signal } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { CartService } from '../../../../core/services/cart/cart-service';
import { CurrencyPipe } from '@angular/common';

@Component({
  selector: 'app-checkout',
  imports: [CurrencyPipe, ReactiveFormsModule],
  templateUrl: './checkout.html',
  styleUrl: './checkout.scss',
})
export class Checkout {
  private fb = inject(FormBuilder);
  private router = inject(Router);
  private cartService = inject(CartService);

  cartCount = this.cartService.cartCount;
  subTotal = this.cartService.subTotal;

  isProcessing = signal(false);
  checkoutForm = this.fb.group({
    firstName: ['', Validators.required],
    lastName: ['', Validators.required],
    email: ['', [Validators.required, Validators.email]],
    address: ['', Validators.required],
    city: ['', Validators.required],
    zip: ['', Validators.required],
    paymentMethod: ['card', Validators.required] // Default to Card
  });

  placeOrder() {
    if (this.checkoutForm.invalid) {
      this.checkoutForm.markAllAsTouched(); // Show errors
      return;
    }

    this.isProcessing.set(true);

    setTimeout(() => {
      const orderId = 'ORD-' + Math.floor(Math.random() * 1000000);

      // Senior Step: Clear the cart locally after success
      this.cartService.clearCart();

      this.isProcessing.set(false);
      this.router.navigate(['/order-confirmation', orderId]);
    }, 2000);
  }
}

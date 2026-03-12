import { Component, inject, signal } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { CartService } from '../../../../core/services/cart/cart-service';
import { CurrencyPipe } from '@angular/common';
import { OrderService } from '../../../../core/services/order/order.service';
import { OrderRequest } from '../../../../core/models/order.model';

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
  private orderService = inject(OrderService);

  cartCount = this.cartService.cartCount;
  subTotal = this.cartService.subTotal;

  isProcessing = signal(false);
  errorMessage = signal('');

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
    this.errorMessage.set('');

    const formValues = this.checkoutForm.value;

    const orderItems = this.cartService.cartItems().map(item => ({
      productId: item.productId,
      productName: item.productName,
      unitPrice: item.unitPrice,
      pictureUrl: '',
      units: item.quantity
    }));

    const request: OrderRequest = {
      shippingAddress: {
        street: formValues.address!,  
        city: formValues.city!,
        zipCode: formValues.zip!
      },
      orderItems: orderItems
    };

    this.orderService.placeOrder(request).subscribe({
      next: (response) => {
        this.isProcessing.set(false);
        this.cartService.clearCart();

        // Remove quotes if the API returned '"xxxx-xxxx-..."'
        const cleanOrderId = response.replace(/['"]+/g, '');
        this.router.navigate(['/order-confirmation', cleanOrderId]);
      },
      error: (err) => {
        this.isProcessing.set(false);
        this.errorMessage.set(err);
        console.error('Order failed', err);
      }
    });
  }
}

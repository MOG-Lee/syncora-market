import { Component, inject } from '@angular/core';
import { CartService } from '../../../../core/services/cart/cart-service';
import { ProductImgPipe } from '../../../../core/pipes/product-img-pipe';
import { CurrencyPipe } from '@angular/common';
import { Router, RouterLink } from "@angular/router";

@Component({
  selector: 'app-cart-drawer',
  imports: [ProductImgPipe, CurrencyPipe, RouterLink],
  templateUrl: './cart-drawer.html',
  styleUrl: './cart-drawer.scss',
})
export class CartDrawer {
  private router = inject(Router); // Inject it
  cartService = inject(CartService);

  items = this.cartService.cartItems;
  total = this.cartService.subTotal;
  isOpen = this.cartService.isOpen;

  close() {
    this.cartService.isOpen.set(false);
  }

  increase(id: string, currentQty: number) {
    this.cartService.updateQuantity(id, currentQty + 1);
  }

  decrease(id: string, currentQty: number) {
    this.cartService.updateQuantity(id, currentQty - 1);
  }

  remove(id: string) {
    this.cartService.removeItem(id);
  }

  proceedToCheckout(){
    this.close();
    this.router.navigate(['/checkout']);
  }
}

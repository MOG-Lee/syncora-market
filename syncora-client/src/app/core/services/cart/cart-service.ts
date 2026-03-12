import { computed, effect, inject, Injectable, signal } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../../../environments/environment';
import { Product } from '../../models/product.model';
import { BasketItem, CustomerBasket } from '../../models/basket.model';
import { AuthService } from '../auth/auth.service';

@Injectable({
  providedIn: 'root',
})
export class CartService {
  private http = inject(HttpClient);
  private authService = inject(AuthService);
  private apiUrl = `${environment.basketApiUrl}/basket`;

  cartItems = signal<BasketItem[]>([]);
  isOpen = signal(false);

  cartCount = computed(() =>
    this.cartItems().reduce((acc, item) => acc + item.quantity, 0)
  );

  subTotal = computed(() =>
    this.cartItems().reduce((acc, item) => acc + (item.unitPrice * item.quantity), 0)
  );

  constructor() {
    // Automatically sync the cart state based on login status
    effect(() => {
      const userEmail = this.authService.currentUser();
      if (userEmail) {
        // User is logged in (or refreshed), fetch their basket from Redis
        this.loadBasket();
      } else {
        // User logged out, clear the local cart
        this.cartItems.set([]);
      }
    });
  }

  toggleDrawer() {
    this.isOpen.update(val => !val);
  }

  private loadBasket() {
    this.http.get<CustomerBasket>(this.apiUrl).subscribe({
      next: (basket) => {
        if (basket && basket.items) {
          this.cartItems.set(basket.items);
        }
      },
      error: (err) => console.error('Failed to load basket from Redis', err)
    });
  }

  private saveBasket() {
    const basket: CustomerBasket = { items: this.cartItems() };
    this.http.post<CustomerBasket>(this.apiUrl, basket).subscribe({
      error: (err) => console.error('Failed to save basket to Redis', err)
    });
  }

  addToCart(product: Product) {
    this.cartItems.update(items => {
      const existing = items.find(i => i.productId === product.id);

      if (existing) {
        return items.map(i =>
          i.productId === product.id
            ? { ...i, quantity: i.quantity + 1 }
            : i
        );
      } else {
        const newItem: BasketItem = {
          productId: product.id,
          productName: product.name,
          unitPrice: product.price,
          quantity: 1,
          pictureUrl: product.imageFileName
        };
        return [...items, newItem];
      }
    });

    this.saveBasket(); // Sync with Redis
    this.isOpen.set(true);
  }

  updateQuantity(productId: string, quantity: number) {
    if (quantity <= 0) {
      this.removeItem(productId);
      return;
    }

    this.cartItems.update(items =>
      items.map(item => item.productId === productId ? { ...item, quantity } : item)
    );
    this.saveBasket(); // Sync with Redis
  }

  removeItem(productId: string) {
    this.cartItems.update(items => items.filter(i => i.productId !== productId));
    this.saveBasket(); // Sync with Redis
  }

  clearCart() {
    this.cartItems.set([]);

    this.http.delete(this.apiUrl).subscribe({
      error: (err) => console.error('Failed to clear basket in Redis', err)
    });
  }
}
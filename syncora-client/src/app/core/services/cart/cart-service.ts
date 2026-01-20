import { computed, Injectable, signal } from '@angular/core';
import { CartItem } from '../../models/cartItem.model';
import { Product } from '../../models/product.model';

@Injectable({
  providedIn: 'root',
})
export class CartService {
  cartItems = signal<CartItem[]>([]);
  isOpen = signal(false);

  cartCount = computed(() =>
    this.cartItems().reduce((accumulator, currentItem) => accumulator + currentItem.quantity, 0)
  );

  subTotal = computed(() =>
    this.cartItems().reduce((accumulator, currentItem) => accumulator + (currentItem.product.price * currentItem.quantity), 0)
  );

  toggleDrawer() {
    this.isOpen.update(val => !val);
  }

  addToCart(product: Product) {
    this.cartItems.update(items => {

      const existing = items.find(i => i.product.id === product.id);

      if (existing) {
        return items.map(i =>
          i.product.id === product.id
            ? { ...i, quantity: i.quantity + 1 }
            : i
        );
      } else {
        return [...items, { product, quantity: 1 }];
      }
    });

    this.isOpen.set(true);
  }

  removeItem(productId: string) {
    this.cartItems.update(items => items.filter(i => i.product.id !== productId));
  }

  updateQuantity(productId: string, quantity: number) {
    if (quantity <= 0) {
      this.removeItem(productId);
      return;
    }

    this.cartItems.update(items =>
      items.map(i => i.product.id === productId ? { ...i, quantity } : i)
    );
  }

  clearCart() {
    this.cartItems.set([]);
  }
}

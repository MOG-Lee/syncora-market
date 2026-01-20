import { Component, inject, signal } from '@angular/core';
import { CartService } from '../../../../core/services/cart/cart-service';
import { ActivatedRoute } from '@angular/router';
import { Product } from '../../../../core/models/product.model';
import { ProductImgPipe } from '../../../../core/pipes/product-img-pipe';
import { CurrencyPipe } from '@angular/common';

@Component({
  selector: 'app-product-details',
  imports: [ProductImgPipe, CurrencyPipe],
  templateUrl: './product-details.html',
  styleUrl: './product-details.scss',
})
export class ProductDetails {
  private route = inject(ActivatedRoute);
  private cartService = inject(CartService);

  isLoading = signal(true);
  product = signal<Product | null>(null);

  ngOnInit() {
    const id = this.route.snapshot.paramMap.get('id');

    if (id) {
      this.loadProduct(id);
    }
  }

  addToCart() {
    const currentProduct = this.product();
    if (currentProduct) {
      this.cartService.addToCart(currentProduct);
    }
  }

  private loadProduct(id: string) {
    setTimeout(() => {
      this.product.set({
        id: id,
        name: 'Sony WH-1000XM5 Wireless Noise Canceling Headphones',
        price: 348.00,
        description: 'The WH-1000XM5 headphones rewrite the rules for distraction-free listening. Two processors control 8 microphones for unprecedented noise cancellation and exceptional call quality.',
        imageUrl: 'a1b2-c3d4.jpg',
        stockQuantity: 15,
        vendorId: 'v1',
        vendorName: 'Sony Official',
        category: 'Audio',
        averageRating: 4.8,
        isOnFlashSale: true
      });
      this.isLoading.set(false);
    }, 1000);
  }
}

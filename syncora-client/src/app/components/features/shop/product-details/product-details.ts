import { Component, inject, signal } from '@angular/core';
import { CartService } from '../../../../core/services/cart/cart-service';
import { ActivatedRoute } from '@angular/router';
import { Product } from '../../../../core/models/product.model';
import { ProductImgPipe } from '../../../../core/pipes/product-img-pipe';
import { CurrencyPipe } from '@angular/common';
import { CatalogService } from '../../../../core/services/catalog/catalog-service';

@Component({
  selector: 'app-product-details',
  imports: [ProductImgPipe, CurrencyPipe],
  templateUrl: './product-details.html',
  styleUrl: './product-details.scss',
})
export class ProductDetails {
  private route = inject(ActivatedRoute);
  private cartService = inject(CartService);
  private catalogService = inject(CatalogService);

  isLoading = signal(true);
  product = signal<Product | null>(null);

  ngOnInit() {
    const id = this.route.snapshot.paramMap.get('id');
    if (id) {
      this.catalogService.getProductById(id).subscribe(data => {
        this.product.set(data);
        this.isLoading.set(false);
      });
    }
  }

  addToCart() {
    const currentProduct = this.product();
    if (currentProduct) {
      this.cartService.addToCart(currentProduct);
    }
  }
}

import { Component, inject, signal } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Product } from '../../../../core/models/product.model';
import { ProductCard } from "../../../shared/product-card/product-card";
import { flatMap } from 'rxjs';
import { NgTemplateOutlet } from '@angular/common';


@Component({
  selector: 'app-product-list',
  imports: [ProductCard, NgTemplateOutlet],
  templateUrl: './product-list.html',
  styleUrl: './product-list.scss',
})
export class ProductList {
  private route = inject(ActivatedRoute);

  searchQuery = signal('');
  products = signal<Product[]>([]);
  isLoading = signal(false);
  sortOption = signal('featured');
  showMobileFilters = signal(false);

  constructor() {
    this.route.queryParams.subscribe(params => {
      const query = params['q'];
      const category = params['category'];

      this.searchQuery.set(query || category || 'All Products');
      this.loadProducts(query, category);
    });
  }

  loadProducts(query: string, category: string) {
    this.isLoading.set(true);

    setTimeout(() => {
      const mockData: Product[] = [
        {
          id: 'p1',
          name: 'Sony WH-1000XM5 Wireless Noise Canceling Headphones',
          price: 348.00,
          description: 'Industry leading noise cancellation.',
          imageUrl: 'a1b2-c3d4.jpg',
          stockQuantity: 15,
          vendorId: 'v1',
          vendorName: 'Sony Official',
          category: 'Audio',
          averageRating: 4.8,
          isOnFlashSale: true
        },
        {
          id: 'p2',
          name: 'MacBook Air M2 Chip',
          price: 999.00,
          description: 'Industry leading noise cancellation.',
          imageUrl: 'a1b2-c3d4.jpg',
          stockQuantity: 15,
          vendorId: 'v1',
          vendorName: 'Sony Official',
          category: 'Audio',
          averageRating: 5.0,
          isOnFlashSale: false
        },
        {
          id: 'p3',
          name: 'Xiaomi Series 5 Fully Noise Canceling Headphones',
          price: 50.00,
          description: 'Industry leading noise cancellation.',
          imageUrl: 'a1b2-c3d4.jpg',
          stockQuantity: 15,
          vendorId: 'v1',
          vendorName: 'Sony Official',
          category: 'Audio',
          averageRating: 4.8,
          isOnFlashSale: true
        },
        {
          id: 'p4',
          name: 'HP OMEN',
          price: 1299.00,
          description: 'Industry leading noise cancellation.',
          imageUrl: 'a1b2-c3d4.jpg',
          stockQuantity: 15,
          vendorId: 'v1',
          vendorName: 'Sony Official',
          category: 'Audio',
          averageRating: 5.0,
          isOnFlashSale: false
        },
      ];

      this.products.set(mockData);
      this.isLoading.set(false);
    }, 800);
  }

  onSortChange(event: Event) {
    const value = (event.target as HTMLSelectElement).value;
    this.sortOption.set(value);

  }

  onPriceChange(event: Event) {

  }

  toggleMobileFilters() {
    this.showMobileFilters.update(val => !val);
  }

  closeMobileFilters() {
    this.showMobileFilters.set(false);
  }
}

import { Component, inject, signal } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Product } from '../../../../core/models/product.model';
import { ProductCard } from "../../../shared/product-card/product-card";
import { flatMap } from 'rxjs';
import { NgTemplateOutlet } from '@angular/common';
import { CatalogService } from '../../../../core/services/catalog/catalog-service';


@Component({
  selector: 'app-product-list',
  imports: [ProductCard, NgTemplateOutlet],
  templateUrl: './product-list.html',
  styleUrl: './product-list.scss',
})
export class ProductList {
  private route = inject(ActivatedRoute);
  private catalogService = inject(CatalogService);

  searchQuery = signal('');
  products = signal<Product[]>([]);
  isLoading = signal(true);
  sortOption = signal('featured');
  showMobileFilters = signal(false);

  constructor() {
    this.route.queryParams.subscribe(params => {
      const query = params['q'];
      const category = params['category'];

      this.searchQuery.set(query || category || 'All Products');
     
    });
  }

  ngOnInit() {
    this.catalogService.getProducts().subscribe(data => {
      this.products.set(data);
      this.isLoading.set(false);
    });
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

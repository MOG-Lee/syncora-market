import { Component, inject, signal } from '@angular/core';
import { ProductCard } from "../../../shared/product-card/product-card";
import { Category, Product } from '../../../../core/models/product.model';
import { ProductImgPipe } from '../../../../core/pipes/product-img-pipe';
import { CatalogService } from '../../../../core/services/catalog/catalog-service';

@Component({
  selector: 'app-home',
  imports: [ProductCard, ProductImgPipe],
  templateUrl: './home.html',
  styleUrl: './home.scss',
})
export class Home {
  private catalogService = inject(CatalogService);

  isLoading = signal(true);
  categories = signal<Category[]>([]);
  products = signal<Product[]>([]);

  ngOnInit() {
    this.catalogService.getCategories().subscribe(data => this.categories.set(data));

    this.catalogService.getProducts().subscribe(data => {
      this.products.set(data);
      this.isLoading.set(false);
    });
  }
}

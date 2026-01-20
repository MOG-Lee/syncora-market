import { Component } from '@angular/core';
import { ProductCard } from "../../../shared/product-card/product-card";
import { Product } from '../../../../core/models/product.model';
import { ProductImgPipe } from '../../../../core/pipes/product-img-pipe';

@Component({
  selector: 'app-home',
  imports: [ProductCard, ProductImgPipe],
  templateUrl: './home.html',
  styleUrl: './home.scss',
})
export class Home {
  isLoading = false;

  categories = [
    { name: 'Electronics', image: 'a1b2-c3d4.jpg' },
    { name: 'Home & Kitchen', image: 'a1b2-c3d4.jpg' },
    { name: 'Gaming Accessories', image: 'a1b2-c3d4.jpg' },
    { name: 'Flash Deals', image: 'a1b2-c3d4.jpg' },
    { name: 'Books', image: 'a1b2-c3d4.jpg' },
    { name: 'Appliances', image: 'a1b2-c3d4.jpg' },
    // { name: 'Groceries', image: 'a1b2-c3d4.jpg' },
    // { name: 'Parfumes', image: 'a1b2-c3d4.jpg' },
    // { name: 'Laptops', image: 'a1b2-c3d4.jpg' },
    // { name: 'Speakers', image: 'a1b2-c3d4.jpg' }
  ]

  products: Product[] = [
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
      name: 'Lenovo Air M2 Chip',
      price: 555.00,
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
      name: 'Kettle Air M2',
      price: 150.00,
      description: 'Industry leading noise cancellation.',
      imageUrl: 'a1b2-c3d4.jpg',
      stockQuantity: 15,
      vendorId: 'v1',
      vendorName: 'Sony Official',
      category: 'Audio',
      averageRating: 3.0,
      isOnFlashSale: false
    },
    {
      id: 'p5',
      name: 'OneOdio Pro 10 Cuffie Over Ear',
      price: 70.00,
      description: 'Industry leading noise cancellation.',
      imageUrl: 'a1b2-c3d4.jpg',
      stockQuantity: 15,
      vendorId: 'v1',
      vendorName: 'Sony Official',
      category: 'Audio',
      averageRating: 2.0,
      isOnFlashSale: false
    },
    {
      id: 'p6',
      name: 'Sony WH-CH520',
      price: 378.00,
      description: 'Industry leading noise cancellation.',
      imageUrl: 'a1b2-c3d4.jpg',
      stockQuantity: 15,
      vendorId: 'v1',
      vendorName: 'Sony Official',
      category: 'Audio',
      averageRating: 3.9,
      isOnFlashSale: true
    },
    {
      id: 'p7',
      name: 'Apple AirPods 4 Auricolari wireless, Auricolari Bluetooth, Audio spaziale personalizzato',
      price: 250.00,
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
      id: 'p8',
      name: 'Soundcore by Anker Q30 Cuffie Bluetooth Cancellazione Rumore Attiva',
      price: 300.00,
      description: 'Industry leading noise cancellation.',
      imageUrl: 'a1b2-c3d4.jpg',
      stockQuantity: 15,
      vendorId: 'v1',
      vendorName: 'Sony Official',
      category: 'Audio',
      averageRating: 3.0,
      isOnFlashSale: false
    },
    {
      id: 'p9',
      name: 'Sony WH-CH720N',
      price: 650.00,
      description: 'Industry leading noise cancellation.',
      imageUrl: 'a1b2-c3d4.jpg',
      stockQuantity: 15,
      vendorId: 'v1',
      vendorName: 'Sony Official',
      category: 'Audio',
      averageRating: 4.8,
      isOnFlashSale: false
    },
    {
      id: 'p10',
      name: 'XIAOMI Redmi Buds 6 Active–Auricolari wireless Bluetooth semi-in-ear,Riduzione del rumore a doppio microfono,Fino a 6h+30h',
      price: 80.00,
      description: 'Industry leading noise cancellation.',
      imageUrl: 'a1b2-c3d4.jpg',
      stockQuantity: 15,
      vendorId: 'v1',
      vendorName: 'Sony Official',
      category: 'Audio',
      averageRating: 1.8,
      isOnFlashSale: false
    }
  ];

}

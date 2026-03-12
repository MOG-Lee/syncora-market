import { Component, inject, signal } from '@angular/core';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { OrderService } from '../../../../core/services/order/order.service';
import { OrderStatus } from '../../../../core/models/order.model';

@Component({
  selector: 'app-order-success',
  imports: [RouterLink],
  templateUrl: './order-success.html',
  styleUrl: './order-success.scss',
})
export class OrderSuccess {
  private route = inject(ActivatedRoute);
  private orderService = inject(OrderService);
  orderId = signal('');
  orderStatus = signal<string | null>(null);
  isLoading = signal(true);
  OrderStatus = OrderStatus;

  ngOnInit() {
    const id = this.route.snapshot.paramMap.get('id');
    if (id) {
      this.orderId.set(id);
      this.fetchOrderDetails(id);
    }
  }

  fetchOrderDetails(id: string) {
    this.isLoading.set(true);
    this.orderService.getOrder(id).subscribe({
      next: (order) => {
        this.orderStatus.set(order.status as string);
        this.isLoading.set(false);
      },
      error: (err) => {
        console.error('Failed to load live order details', err);
        this.isLoading.set(false);
      }
    });
  }
}
import { Component, inject, signal } from '@angular/core';
import { ActivatedRoute, RouterLink } from '@angular/router';

@Component({
  selector: 'app-order-success',
  imports: [RouterLink],
  templateUrl: './order-success.html',
  styleUrl: './order-success.scss',
})
export class OrderSuccess {
  private route = inject(ActivatedRoute);
  orderId = signal('');

  ngOnInit() {
    const id = this.route.snapshot.paramMap.get('id');
    this.orderId.set(id || 'UNKNOWN');
  }
}

import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../../../environments/environment';
import { Observable } from 'rxjs';
import { OrderRequest, OrderDetail } from '../../models/order.model';

@Injectable({
  providedIn: 'root'
})
export class OrderService {
  private http = inject(HttpClient);
  private apiUrl = `${environment.orderingApiUrl}/orders`;

  placeOrder(orderRequest: OrderRequest): Observable<any> {
    return this.http.post(this.apiUrl, orderRequest, { responseType: 'text' });
  }

  getOrder(id: string): Observable<OrderDetail> {
    return this.http.get<OrderDetail>(`${this.apiUrl}/${id}`);
  }
}

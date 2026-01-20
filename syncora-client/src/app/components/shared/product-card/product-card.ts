import { Component, inject, Input } from '@angular/core';
import { Product } from '../../../core/models/product.model';
import { RouterLink } from "@angular/router";
import { ProductImgPipe } from "../../../core/pipes/product-img-pipe";
import { CurrencyPipe } from '@angular/common';
import { CartService } from '../../../core/services/cart/cart-service';

@Component({
  selector: 'app-product-card',
  imports: [RouterLink, ProductImgPipe, CurrencyPipe],
  templateUrl: './product-card.html',
  styleUrl: './product-card.scss',
})
export class ProductCard {
  @Input({ required: true }) product!: Product;

  private cartService = inject(CartService);
  constructor() {

  }

  addToCart(event: Event) {
    // Senior Tip: Stop the click from bubbling up to the <a> tag wrapping the card.
    // Otherwise, clicking 'Add' would also open the Product Details page.

    //***** we do not need these two lines of code, the button is not inside <a> (child), it is neighbor to it.
    //***** "click event" bubbles up from "Button -> Div -> Card Body -> Card Div" and never hit <a>
    // event.stopPropagation();
    // event.preventDefault();

    this.cartService.addToCart(this.product);
  }
}

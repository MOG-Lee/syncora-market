import { Component, inject } from '@angular/core';
import { Router, RouterLink } from "@angular/router";
import { CartService } from '../../../core/services/cart/cart-service';

@Component({
  selector: 'app-navbar',
  imports: [RouterLink],
  templateUrl: './navbar.html',
  styleUrl: './navbar.scss',
})
export class Navbar {
  private router = inject(Router);
  private cartService = inject(CartService);

  cartCount = this.cartService.cartCount;

  onSearch(term: string): void {
    const trimmedTerm = term.trim();

    if (trimmedTerm) {
      this.router.navigate(['/search'], {
        queryParams: { q: trimmedTerm }
      });
    }
  }

  toggleCart(){
    this.cartService.toggleDrawer();
  }
  
}

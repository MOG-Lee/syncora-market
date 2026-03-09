import { Component, computed, inject } from '@angular/core';
import { Router, RouterLink } from "@angular/router";
import { CartService } from '../../../core/services/cart/cart-service';
import { AuthService } from '../../../core/services/auth/auth.service';

@Component({
  selector: 'app-navbar',
  imports: [RouterLink],
  templateUrl: './navbar.html',
  styleUrl: './navbar.scss',
})
export class Navbar {
  private router = inject(Router);
  private cartService = inject(CartService);
  private authService = inject(AuthService);

  cartCount = this.cartService.cartCount;
  currentUser = this.authService.currentUser;

  displayName = computed(() => {
    const email = this.currentUser();
    if (!email)
      return null;

    return email.split('@')[0];
  });

  isAdmin = computed(() => this.authService.hasRole(['Admin', 'SuperAdmin']));

  onSearch(term: string): void {
    const trimmedTerm = term.trim();

    if (trimmedTerm) {
      this.router.navigate(['/search'], {
        queryParams: { q: trimmedTerm }
      });
    }
  }

  toggleCart() {
    this.cartService.toggleDrawer();
  }

  logout(event: Event) {
    event.preventDefault();
    this.authService.logout();
    this.router.navigate(['/login']);
  }
}

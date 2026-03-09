import { CanActivateFn, Router } from "@angular/router";
import { AuthService } from "../services/auth/auth.service";
import { inject } from "@angular/core";
import { UserRole } from "../models/auth.model";


export const roleGuard: CanActivateFn = (route, state) => {
  const authService = inject(AuthService);
  const router = inject(Router);

  // Read the expected roles from the route configuration data
  const expectedRoles = route.data?.['roles'] as UserRole[];

  if (!authService.currentUser()) {
    router.navigate(['/login']);
    return false;
  }

  if (expectedRoles && !authService.hasRole(expectedRoles)) {
    // User is logged in, but doesn't have the right role
    router.navigate(['/']); 
    return false;
  }

  return true;
};
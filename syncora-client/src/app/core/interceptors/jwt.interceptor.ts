import { HttpInterceptorFn } from "@angular/common/http";
import { AuthService } from "../services/auth/auth.service";
import { inject } from "@angular/core";

export const jwtInterceptor: HttpInterceptorFn = (req, next) => {
  const authService = inject(AuthService);
  const token = authService.getToken();

  // If we have a token, clone the request and attach the Authorization header
  if (token) {
    const authReq = req.clone({
      headers: req.headers.set('Authorization', `Bearer ${token}`)
    });
    
    // Pass the modified request to the next handler
    return next(authReq);
  }

  // If there is no token just pass the original request through untouched.
  return next(req);
};

import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { AuthService } from '../services/auth.service';

export const authGuard: CanActivateFn = () => {
  const authService = inject(AuthService);
  const router = inject(Router);

  const token = authService.getAccessToken();
  const decoded = authService.getDecodedToken();
  const now = Math.floor(Date.now() / 1000);

  if (!token || !decoded || decoded.exp < now) {
    router.navigate(['/login']);
    return false;
  }

  return true;
};

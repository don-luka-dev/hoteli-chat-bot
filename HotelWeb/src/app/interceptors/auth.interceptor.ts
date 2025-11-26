import {
  HttpInterceptorFn,
  HttpRequest,
  HttpHandlerFn,
  HttpErrorResponse,
  HttpStatusCode,
} from '@angular/common/http';
import { inject } from '@angular/core';
import { AuthService } from '../services/auth.service';
import { catchError, switchMap, throwError } from 'rxjs';
import { Router } from '@angular/router';

export const authInterceptor: HttpInterceptorFn = (
  req: HttpRequest<unknown>,
  next: HttpHandlerFn
) => {
  const authService = inject(AuthService);
  const router = inject(Router);

  const accessToken = localStorage.getItem('accessToken');

  let authReq = req;
  if (accessToken) {
    authReq = req.clone({
      setHeaders: {
        Authorization: `Bearer ${accessToken}`,
      },
    });
  }

  return next(authReq).pipe(
    catchError((error: HttpErrorResponse) => {
      if (error.status === HttpStatusCode.Unauthorized) {
        const refreshToken = localStorage.getItem('refreshToken');
        const userId = localStorage.getItem('userId');

        if (!refreshToken || !userId) {
          router.navigate(['/login']);
          return throwError(() => error);
        }

        return authService
          .refreshTokens({ refreshToken, userId: +userId })
          .pipe(
            switchMap((tokenResponse) => {
              localStorage.setItem('accessToken', tokenResponse.accessToken);
              localStorage.setItem('refreshToken', tokenResponse.refreshToken);

              const newReq = req.clone({
                setHeaders: {
                  Authorization: `Bearer ${tokenResponse.accessToken}`,
                },
              });

              return next(newReq);
            }),
            catchError((refreshError) => {
              localStorage.clear();
              router.navigate(['/login']);
              return throwError(() => refreshError);
            })
          );
      }

      return throwError(() => error);
    })
  );
};

import { Injectable } from '@angular/core';
import {
  HttpEvent,
  HttpInterceptor,
  HttpHandler,
  HttpRequest,
  HttpErrorResponse
} from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';

@Injectable()
export class ErrorInterceptor implements HttpInterceptor {
  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    return next.handle(req).pipe(
      catchError((error: HttpErrorResponse) => {
        let errorMsg = '';

        if (error.status === 0) {
          errorMsg = 'Não foi possível conectar ao servidor. Verifique se o backend está rodando.';
        } else if (error.error) {
          if (typeof error.error === 'string') {
            errorMsg = error.error;
          } else if (error.error.message) {
            errorMsg = error.error.message;
          } else {
            errorMsg = JSON.stringify(error.error);
          }
        } else {
          errorMsg = error.message;
        }

        // Pode personalizar ou logar o erro aqui

        return throwError(() => new Error(errorMsg));
      })
    );
  }
}

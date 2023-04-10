
2
3
4
5
6
7
8
9
10
11
12
13
14
15
16
17
18
19
20
21
22
23
24
25
26
27
28
29
30
import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor,
  HttpErrorResponse,
  HttpEventType
} from '@angular/common/http';
import { Observable, catchError } from 'rxjs'; 
import { LocalStorageService } from '../services/localstorage.service'
import { AuthLoginService } from '../modules/auth/services/auth-login.service'

@Injectable()
export class AuthInterceptor implements HttpInterceptor {

  constructor(
    private authService: AuthLoginService
    ) {

    }

  intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    return next
      .handle(this.addAuthToken(request))
      .pipe(
        catchError((error: HttpErrorResponse) => { 
          if (error.status === 401) {
            this.authService.logout();
          }
          throw error;
        })
      )
    }

  addAuthToken(request: HttpRequest<any>) {  
    if  (!this.authService.isLoggedIn()) {
      return request;
    }

    const token = this.authService.getToken(); 
    return request.clone({
        setHeaders: {
          Authorization: `Bearer ${token}`
        }
    })
  }
}
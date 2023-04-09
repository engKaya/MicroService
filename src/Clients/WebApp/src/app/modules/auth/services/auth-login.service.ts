import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { BehaviorSubject, Observable, Subject, lastValueFrom, of, throwError } from 'rxjs';
import { retry, catchError } from 'rxjs/operators';
import {
  LoginRequestModel,
  LoginResponseModel,
} from '../models/loginRequest.model';
import { environment } from 'src/enviroment/enviroment';
@Injectable({
  providedIn: 'root',
})
export class AuthLoginService {

  apiUrl = environment.api_gateway;
  
    
  IsLoadingSubject: BehaviorSubject<boolean>;
  IsLoading$: Observable<boolean>;

  constructor(private http: HttpClient) {
    this.IsLoadingSubject = new BehaviorSubject<boolean>(false);
    this.IsLoading$ = this.IsLoadingSubject.asObservable();
  }

  login(model: LoginRequestModel): Promise<LoginResponseModel> {
    this.IsLoadingSubject.next(true);    
    return lastValueFrom (this.http
        .post<LoginResponseModel>(`${this.apiUrl}auth`, model)
        .pipe(retry(1), catchError(this.handleError))).finally(() => {
            this.IsLoadingSubject.next(false);
        });
  }

  handleError(error : any) {
    let errorMessage = '';
    if (error.error instanceof ErrorEvent) {
      // Get client-side error
      errorMessage = error.error.message;
    } else {
      // Get server-side error
      errorMessage = `Error Code: ${error.status}\nMessage: ${error.message}`;
    }
    window.alert(errorMessage);
    return throwError(() => {
      return errorMessage;
    });
  }
}

import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpStatusCode } from '@angular/common/http';
import {
  BehaviorSubject,
  Observable,
  Subject,
  lastValueFrom,
  of,
  throwError,
} from 'rxjs';
import { retry, catchError } from 'rxjs/operators';
import {
  LoginRequestModel,
  LoginResponseModel,
} from '../models/loginRequest.model';
import { environment } from 'src/enviroment/enviroment';
import { LocalStorageService } from 'src/app/services/localstorage.service';
@Injectable({
  providedIn: 'root',
})
export class AuthLoginService {
  apiUrl = environment.api_gateway;
  isDevMode = environment.isDevMode;

  IsLoadingSubject: BehaviorSubject<boolean>;
  IsLoading$: Observable<boolean>;
 
  IsLoggedIn$: Observable<boolean>;
  IsLoggedInSubject: Subject<boolean>;

  UserName$: Observable<string>;
  UserNameSubject: Subject<string>;


  constructor(
    private http: HttpClient,
    private localStorage: LocalStorageService
  ) {
    this.IsLoadingSubject = new BehaviorSubject<boolean>(false);
    this.IsLoading$ = this.IsLoadingSubject.asObservable(); 

    this.IsLoggedInSubject = new Subject<boolean>();
    this.IsLoggedIn$ = this.IsLoggedInSubject.asObservable();

    this.UserNameSubject = new Subject<string>();
    this.UserName$ = this.UserNameSubject.asObservable();
  }

  login(model: LoginRequestModel): Promise<LoginResponseModel> {
    this.IsLoadingSubject.next(true);
    return lastValueFrom(
      this.http.post<LoginResponseModel>(`${this.apiUrl}auth`, model)
    )
      .then((response) => {
        if (response.Status === HttpStatusCode.Ok) { 
          this.localStorage.SetToken(response.Token as string);
          this.localStorage.setUsername(response.UserName as string); 
          this.IsLoggedInSubject.next(true);
          this.UserNameSubject.next(response.UserName as string);
        }
        return response;
      })
      .finally(() => {
        this.IsLoadingSubject.next(false);
      })
      .catch((error) => {
        if (this.isDevMode) this.handleError(error);
        return error;
      });
  }

  handleError(error: any) {
    let errorMessage = '';
    if (error.error instanceof ErrorEvent) {
      errorMessage = error.error.message;
    } else {
      errorMessage = `Error Code: ${error.status}\nMessage: ${error.message}`;
    }
    window.alert(errorMessage);
    return throwError(() => {
      return errorMessage;
    });
  }

  logout() {
    this.localStorage.RemoveToken();
    this.localStorage.RemoveUsername(); 
  }

  IsLogged = this.isLoggedIn();
  userName = this.getUserName();

  isLoggedIn(): boolean { 
    return this.localStorage.GetToken() !== null && this.localStorage.GetToken() !== undefined && this.localStorage.GetToken() !== '';
  }

  getUserName(): string {
    return this.localStorage.getUsername();
  }
}

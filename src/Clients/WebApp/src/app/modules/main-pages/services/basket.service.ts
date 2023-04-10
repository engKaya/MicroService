import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable, lastValueFrom, throwError } from 'rxjs';
import { environment } from 'src/enviroment/enviroment';
import { BasketItem } from '../objects/models/BasketItem.model';
import { ToasterService } from 'src/app/services/toaster.service';

const API_URL = environment.api_gateway;

@Injectable({
  providedIn: 'root',
})
export class BasketService {
  IsLoadingSubject: BehaviorSubject<boolean>;
  IsLoading$: Observable<boolean>;
 
  constructor(
    private http: HttpClient,
    private toastr: ToasterService,
    ) {
    this.IsLoadingSubject = new BehaviorSubject<boolean>(false);
    this.IsLoading$ = this.IsLoadingSubject.asObservable();

    this.refreshPageMessage = new BehaviorSubject<boolean>(false);
    this.refreshPageMessage$ = this.refreshPageMessage.asObservable();
  }

  refreshPageMessage = new BehaviorSubject<boolean>(false);
  refreshPageMessage$ = this.refreshPageMessage.asObservable();

  getBasketCount(): Promise<number> {
    let url = `${API_URL}basket/GetBasketCount`;
    return lastValueFrom(this.http.get<number>(url)).catch((error) => {
      this.handleError(error);
      return error;
    });
  }
  addToCart(item: BasketItem) {
    this.IsLoadingSubject.next(true);
    let url = `${API_URL}basket/AddItem`;
    return lastValueFrom(this.http.post(url, item))
      .finally(() => {
        this.IsLoadingSubject.next(false);
        this.refreshPage();
      })
      .catch((error) => {
        this.handleError(error);
        return error;
      });
  }

  handleError(error: any) {
    if (!environment.isDevMode) return;
    let errorMessage = '';
    if (error.error instanceof ErrorEvent) {
      errorMessage = error.error.message;
    } else {
      errorMessage = `Error Code: ${error.status}\nMessage: ${error.message}`;
    }
    this.toastr.openToastError("Error",errorMessage);
    return throwError(() => {
      return errorMessage;
    });
  }

  refreshPage() {
    this.refreshPageMessage.next(true);
  }
}

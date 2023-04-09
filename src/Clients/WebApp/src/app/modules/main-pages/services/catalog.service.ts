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
import { environment } from 'src/enviroment/enviroment';
import { LocalStorageService } from 'src/app/services/localstorage.service';
import { PaginatedViewModel } from 'src/app/common-objects/PaginatedViewModel.model';
import { CatalogItem } from '../objects/entities/CatalogItem.model';

@Injectable({
  providedIn: 'root',
})


export class CatalogService {

    IsLoadingSubject: BehaviorSubject<boolean>;
    IsLoading$: Observable<boolean>;
    API_URL = environment.api_gateway;
    constructor(
        private http: HttpClient,
        private localStorage: LocalStorageService
    ) { 
        this.IsLoadingSubject = new BehaviorSubject<boolean>(false);
        this.IsLoading$ = this.IsLoadingSubject.asObservable();        
    }

    getItems(): Promise<PaginatedViewModel<CatalogItem>>{
        this.IsLoadingSubject.next(true);
        let url = `${this.API_URL}api/catalog/items`;
        return lastValueFrom(this.http.get<PaginatedViewModel<CatalogItem>>(url)).finally(() => {
            this.IsLoadingSubject.next(false);
        }).catch((error) => {
            if (environment.isDevMode) this.handleError(error);
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
}
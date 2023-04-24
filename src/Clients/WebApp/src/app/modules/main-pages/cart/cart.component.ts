import { ChangeDetectorRef, Component } from '@angular/core';
import { BasketService } from '../services/basket.service';
import { Router } from '@angular/router';
import { Observable } from 'rxjs';
import { CustomerBasket } from '../objects/entities/CustomerBasket.model';
import { BasketItem } from '../objects/models/BasketItem.model';
import { environment } from 'src/enviroment/enviroment';
import { BasketCheckout } from '../objects/models/BasketCheckout.model';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { MatDatepicker } from '@angular/material/datepicker';  
import {MomentDateAdapter, MAT_MOMENT_DATE_ADAPTER_OPTIONS} from '@angular/material-moment-adapter';
import {DateAdapter, MAT_DATE_FORMATS, MAT_DATE_LOCALE} from '@angular/material/core';
import { TranslateService } from '@ngx-translate/core';

@Component({
  selector: 'app-cart',
  templateUrl: './cart.component.html'
})
export class CartComponent {
  constructor(
    private basketService: BasketService,
    private ref: ChangeDetectorRef,
    private translate: TranslateService,
    private router: Router,
  ) { }

  picture_url: string = `${environment.catalog_api}Pics/`;
  basketItems: CustomerBasket= new CustomerBasket();

  form : FormGroup = new FormGroup({

    City : new FormControl('', [Validators.required, Validators.minLength(3), Validators.maxLength(5)]),
    Street : new FormControl('', [Validators.required]),
    State : new FormControl('', [Validators.required]),
    ZipCode : new FormControl('', [Validators.required]), 
    CardNumber : new FormControl('', [Validators.required, Validators.minLength(16), Validators.maxLength(16)]),
    CardHolderName : new FormControl('', [Validators.required]), 
    Expiration : new FormControl('', [Validators.required]),
  });
  
  IsLoading$ : Observable<boolean> = new Observable<boolean>();

  ngOnInit(): void {
    this.IsLoading$ = this.basketService.IsLoading$;
    this.getBasket();
  }

  total: number = 0;

  calculateTotal() {
    this.basketItems?.Items?.forEach((item) => {
      this.total += item.UnitPrice * item.Quantity;
    });
  }

  getBasket() {
    this.basketService.getBasketItems().then((basket) => {
      this.basketItems = basket; 
      this.calculateTotal();
      this.ref.detectChanges();
    });
  }
  removeProductFromCard(item: BasketItem) {
    this.basketService.deleteBasketItem(item.ProductId).then(() => {
      this.getBasket();
      this.basketService.refreshPage();
    });
  }
  updateBasketItem(item: any, e: any) {
    item.quantity = e.target.value; 
    this.basketService.updateBasket(this.basketItems).then(() => {
      this.getBasket();
    });
  }

  setMonthAndYear(e: any, datepicker: MatDatepicker<any>) {

  }

  basketCheckout : BasketCheckout = new BasketCheckout();

  submit() {
  }

  
  getErrorMessage(control: string) {    
    if(this.form.get(control)?.errors?.['required'])
      return this.translate.instant('FORM_VALID.REQUIRED');
      
    if(this.form.get(control)?.errors?.['minlength'])
      return this.translate.instant('FORM_VALID.MINLENGTH', {minl: this.form.get(control)?.errors?.['minlength']?.requiredLength}); 

    if(this.form.get(control)?.errors?.['maxlength'])
      return this.translate.instant('FORM_VALID.MAXLENGTH', {maxl: this.form.get(control)?.errors?.['maxlength']?.requiredLength});

    return "Invalid value";
  }
}

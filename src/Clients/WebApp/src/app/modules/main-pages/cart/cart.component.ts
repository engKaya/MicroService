import { ChangeDetectorRef, Component } from '@angular/core';
import { BasketService } from '../services/basket.service';
import { Router } from '@angular/router';
import { Observable } from 'rxjs';
import { CustomerBasket } from '../objects/entities/CustomerBasket.model';
import { BasketItem } from '../objects/models/BasketItem.model';
import { environment } from 'src/enviroment/enviroment';

@Component({
  selector: 'app-cart',
  templateUrl: './cart.component.html'
})
export class CartComponent {
  constructor(
    private basketService: BasketService,
    private ref: ChangeDetectorRef,
    private router: Router,
  ) { }

  picture_url: string = `${environment.catalog_api}Pics/`;
  basketItems: CustomerBasket= new CustomerBasket();
  
  IsLoading$ : Observable<boolean> = new Observable<boolean>();

  ngOnInit(): void {
    this.IsLoading$ = this.basketService.IsLoading$;
    this.getBasket();
  }



  getBasket() {
    this.basketService.getBasketItems().then((basket) => {
      this.basketItems = basket; 
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
}

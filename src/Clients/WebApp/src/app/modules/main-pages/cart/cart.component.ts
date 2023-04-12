import { ChangeDetectorRef, Component } from '@angular/core';
import { BasketService } from '../services/basket.service';
import { Router } from '@angular/router';
import { Observable } from 'rxjs';
import { CustomerBasket } from '../objects/entities/CustomerBasket.model';

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

  basketItems: CustomerBasket= new CustomerBasket();
  
  IsLoading$ : Observable<boolean> = new Observable<boolean>();

  ngOnInit(): void {
    this.IsLoading$ = this.basketService.IsLoading$;
    this.getBasket();
  }



  getBasket() {
    this.basketService.getBasketItems().then((basket) => {
      this.basketItems = basket;
      console.log(this.basketItems);
      this.ref.detectChanges();
    });
  }

  updateBasketItem(item: any) {
    
    
    // this.basketService.updateBasketItem(item).then(() => {
    //   this.getBasket();
    // });
  }
}

import { ChangeDetectorRef, Component } from '@angular/core';
import { Observable, lastValueFrom, of } from 'rxjs';
import { AuthLoginService } from 'src/app/modules/auth/services/auth-login.service';
import { BasketService } from 'src/app/modules/main-pages/services/basket.service';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: [],
})
export class NavbarComponent {
  IsLogged: boolean = false;
  userName: string = '';

  constructor(
    private authLoginService: AuthLoginService,
    private basketService: BasketService,
    private ref: ChangeDetectorRef,
    
  ) {
    this.authLoginService.IsLoggedIn$.subscribe((isLogged: boolean) => {
      this.IsLogged = isLogged;
      this.getCartCount();
      this.ref.detectChanges();
    });
    this.authLoginService.UserName$.subscribe((userName: string) => {
      this.userName = userName;
      this.ref.detectChanges();
    });

    this.basketService.refreshPageMessage$.subscribe((refresh: boolean) => { 
      this.getCartCount();
    });
  }

  ngOnInit(): void {
    this.IsLogged = this.authLoginService.isLoggedIn(); 
    this.userName = this.authLoginService.getUserName();
    this.getCartCount();
    this.ref.detectChanges();
  }

  cartCount : number = 0;

  getCartCount(){
    if (!this.IsLogged) return;
    this.basketService.getBasketCount().then(res => {   
      this.cartCount = res;
      this.ref.detectChanges();
    });
  }
}

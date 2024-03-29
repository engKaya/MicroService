import { ChangeDetectorRef, Component } from '@angular/core';
import { CatalogService } from '../services/catalog.service';
import { Observable } from 'rxjs';
import { environment } from 'src/enviroment/enviroment';
import { PaginatedViewModel } from 'src/app/common-objects/PaginatedViewModel.model';
import { CatalogItem } from '../objects/entities/CatalogItem.model';
import { BasketItem } from '../objects/models/BasketItem.model';
import { BasketService } from '../services/basket.service';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html'
})
export class DashboardComponent {
  
  picture_url: string = `${environment.catalog_api}Pics/`;

  products: PaginatedViewModel<CatalogItem> = new PaginatedViewModel<CatalogItem>();

  IsItemsLoading: Observable<boolean>;
  IsAddingToCart: Observable<boolean>;
  constructor(
    private catalogService: CatalogService,
    private basketService: BasketService,
    private ref : ChangeDetectorRef
  ) {
    this.IsItemsLoading = catalogService.IsLoading$;
    this.IsAddingToCart = basketService.IsLoading$;
  }

  ngOnInit() {
    this.catalogService.getItems().then(data => { 
      this.products = data;  
      this.ref.detectChanges();
    });
  }

  addToCart(item: CatalogItem) {

    var basketItem = new BasketItem(item);
    this.basketService.addToCart(basketItem);
  }
}

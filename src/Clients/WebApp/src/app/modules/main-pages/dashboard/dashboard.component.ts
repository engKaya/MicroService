import { ChangeDetectorRef, Component } from '@angular/core';
import { CatalogService } from '../services/catalog.service';
import { Observable } from 'rxjs';
import { environment } from 'src/enviroment/enviroment';
import { PaginatedViewModel } from 'src/app/common-objects/PaginatedViewModel.model';
import { CatalogItem } from '../objects/entities/CatalogItem.model';
import { BasketItem } from '../objects/models/BasketItem.model';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html'
})
export class DashboardComponent {
  
  picture_url: string = `${environment.catalog_api}Pics/`;

  products: PaginatedViewModel<CatalogItem> = new PaginatedViewModel<CatalogItem>();

  IsItemsLoading: Observable<boolean>;
  constructor(
    private catalogService: CatalogService,
    private ref : ChangeDetectorRef
  ) {
    this.IsItemsLoading = catalogService.IsLoading$;
  }

  ngOnInit() {
    this.catalogService.getItems().then(data => {
      this.products = data; 
      console.log(this.products);
      this.ref.detectChanges();
    });
  }

  addToCart(item: CatalogItem) {

    var basketItem = new BasketItem(item);
    this.catalogService.addToCart(basketItem);
  }
}

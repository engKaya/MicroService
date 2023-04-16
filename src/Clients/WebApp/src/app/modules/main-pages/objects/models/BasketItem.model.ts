import { environment } from "src/enviroment/enviroment";
import { CatalogItem } from "../entities/CatalogItem.model";

const picture_url= `${environment.catalog_api}Pics/`;

export class BasketItem {
    /**
     *
     */
    constructor(catalogItem: CatalogItem) {
        
        this.ProductId = catalogItem.Id;
        this.ProductName = catalogItem.Name;
        this.UnitPrice = catalogItem.Price;
        this.OldUnitPrice = catalogItem.AvailableStock
        this.Quantity = 1;
        this.PictureUrl = picture_url + catalogItem.PictureFileName;
        
    }
    
    Id: string = '';
    ProductId: number;
    ProductName: string;
    UnitPrice: number;
    OldUnitPrice: number;
    Quantity: number;
    PictureUrl: string;
}
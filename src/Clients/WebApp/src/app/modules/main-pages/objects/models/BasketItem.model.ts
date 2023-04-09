import { CatalogItem } from "../entities/CatalogItem.model";

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
        this.PictureUrl = catalogItem.PictureUri;
        
    }
    
    Id: string = '';
    ProductId: number;
    ProductName: string;
    UnitPrice: number;
    OldUnitPrice: number;
    Quantity: number;
    PictureUrl: string;
}
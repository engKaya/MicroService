 export interface CatalogItem {
    Id: number;
    Name: string;
    Description: string;
    Price: number;
    PictureFileName: string;
    PictureUri: string;
    CatalogTypeId: number;
    AvailableStock: number;
    OnReorder: boolean;
    CatalogType: CatalogType;
    CatalogBrandId: number;
    CatalogBrand: CatalogBrand;
}

export interface CatalogType {
    Id: number;
    Type: string;
}

export interface CatalogBrand {
    Id: number;
    Brand: string;
}
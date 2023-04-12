import { BasketItem } from "../models/BasketItem.model";

export class CustomerBasket {
    BuyerId: string = '';
    Items: BasketItem[] = [];
}
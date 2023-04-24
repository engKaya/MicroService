export class BasketCheckout {
     

    City: string = "";
    Street: string = "";
    State: string = "";
    Country: string = "";
    ZipCode: string = "";
    CardNumber: string = "";
    CardHolderName: string = "";
    CardExpiration: string = "";
    CardSecurityNumber: string  = "";
    CardTypeId: CardType = 1;
    BuyerId: string = "";
}

export enum CardType {
    Amex = 1,
    MasterCard = 2,
    Visa = 3
}
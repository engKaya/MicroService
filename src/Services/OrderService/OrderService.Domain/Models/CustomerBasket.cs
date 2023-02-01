using System.Collections.Generic;

namespace OrderService.Domain.Models
{
    public class CustomerBasket
    {
        public CustomerBasket()
        {

        }
        public CustomerBasket(string id)
        {
            this.BuyerId = id;
        }
        public string BuyerId { get; set; }
        public List<BasketItem> Items { get; set; } = new List<BasketItem>();
    }
}

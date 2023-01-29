using System;
using System.Collections.Generic;

namespace OrderService.Application.Features.Queries.ViewModels
{
    public class OrderDetailViewModel
    {
        public string OrderNumber { get; init; }
        public DateTime OrderDate { get; init; }
        public string Status { get; init; }
        public string Description { get; init; }
        public string Street { get; init; }
        public string City { get; init; }
        public string Zipcode { get; init; }
        public string Country { get; init; }
        public List<OrderItem> OrderItems;
        public decimal Total { get; init; }

        public class OrderItem
        {
            public string ProdcutName { get; init; }
            public int Units { get; init; }
            public double UnitPrice { get; init; }
            public string PictureUrl{ get; init; }
        }

    }
}

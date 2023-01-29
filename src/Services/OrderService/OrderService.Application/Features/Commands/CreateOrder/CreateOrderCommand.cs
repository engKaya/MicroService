using BasketService.Api.Core.Domain.Models;
using MediatR;
using OrderService.Application.DTOs.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderService.Application.Features.Commands.CreateOrder
{
    public class CreateOrderCommand : IRequest<bool>
    {
        private readonly List<OrderItemDTO> _orderItems;
        public string UserName { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string ZipCode { get; set; }
        public string CardNumber { get; set; }
        public string CardHolderName { get; set; }
        public DateTime CardExpiration { get; set; }
        public string CardSecurityNumber { get; set; }
        public int CardTypeId { get; set; }
        public IEnumerable<OrderItemDTO> OrderItems { get { return _orderItems.AsReadOnly(); } }
        //public string CorrelationId { get; set; }
        public CreateOrderCommand()
        {
            _orderItems = new List<OrderItemDTO>();
        }

        public CreateOrderCommand(List<BasketItem> basketItems, string userName, string city, string street, string state, string country, string zipcode, string cardNumber, string cardHolderName, DateTime cardExpiration, string cardSecurityNumber, int cardTypeId) : this()
        {
            var DTOList = new List<OrderItemDTO>();
            foreach (var item in basketItems)
            {
                DTOList.Add(new OrderItemDTO { ProductName = item.ProductName, PictureUrl = item.PictureUrl, UnitPrice = item.UnitPrice, Units = item.Quantity });
            }

            _orderItems = DTOList.ToList();
            UserName = userName;
            City = city;
            Street = street;
            State = state;
            Country = country;
            ZipCode = zipcode;
            CardNumber = cardNumber;
            CardHolderName = cardHolderName;
            CardExpiration = cardExpiration;
            CardSecurityNumber = cardSecurityNumber;
            CardTypeId = cardTypeId; 
        }

    }
}

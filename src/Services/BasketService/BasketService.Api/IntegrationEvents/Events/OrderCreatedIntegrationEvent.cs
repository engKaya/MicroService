﻿using BasketService.Api.Core.Domain.Models;
using EventBus.Base.Events;
using System;

namespace BasketService.Api.IntegrationEvents.Events
{
    public class OrderCreatedIntegrationEvent : IntegrationEvent
    {
        public string UserId { get; }
        public string UserName { get; }
        public int OrderNumber { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string ZipCode { get; set; }
        public string CardNumber { get; set; }
        public string CardHolderName{ get; set; }
        public DateTime CardExpiretaion{ get; set; }
        public string CardSecurityNumber { get; set; }
        public int CardTypeId{ get; set; }
        public string BuyerId{ get; set; }
        public Guid RequsetId{ get; set; }
        public CustomerBasket Basket{ get; }
        public OrderCreatedIntegrationEvent(string userId, string userName, string city, string street, string state, string country, string zipCode, string cardNumber, string cardHolderName, DateTime cardExpiretaion, string cardSecurityNumber, int cardTypeId, string buyerId, CustomerBasket basket)
        {
            UserId = userId;
            UserName = userName; 
            City = city;
            Street = street;
            State = state;
            Country = country;
            ZipCode = zipCode;
            CardNumber = cardNumber;
            CardHolderName = cardHolderName;
            CardExpiretaion = cardExpiretaion;
            CardSecurityNumber = cardSecurityNumber;
            CardTypeId = cardTypeId;
            BuyerId = buyerId; 
            Basket = basket;
        }
    }
}
using OrderService.Domain.AggregateModels.BuyerAggregate;
using OrderService.Domain.Events;
using OrderService.Domain.SeedWork;
using System;
using System.Collections.Generic;

namespace OrderService.Domain.AggregateModels.OrderAggregate
{
    public class Order : BaseEntity, IAggragateRoot
    {
        public DateTime OrderDate { get; private set; }
        public int Quantity { get; private set; }
        public string Description { get; private set; }
        public Guid? BuyerId { get; private set; }
        public Buyer Buyer { get; private set; }
        public Address Address { get; private set; }
        private int _orderStatusId;
        public OrderStatus OrderStatus { get; private set; }
        private readonly List<OrderItem> _orderItems;
        public IReadOnlyCollection<OrderItem> OrderItems => _orderItems.AsReadOnly();
        public Guid? PaymentMethodId { get; set; }

        protected Order()
        {
            Id = Guid.NewGuid();
            _orderItems = new List<OrderItem>();
        }

        public Order(
                string UserName, 
                Address address, 
                int cardTypeId, 
                string cardNumber, 
                string securityNumber, 
                string cardHolderName, 
                DateTime cardExpiration,
                Guid? buyerId = null)
            : this()
        {
            BuyerId = buyerId;
            OrderDate = DateTime.UtcNow;
            _orderStatusId = OrderStatus.Submitted.Id;
            Address = address;

            AddOrderStartedDomainEvent(UserName, cardTypeId, cardNumber, securityNumber, cardHolderName, cardExpiration);
        }

        private void AddOrderStartedDomainEvent(string UserName, int cardTypeId, string cardNumber, string securityNumber, string cardHolderName, DateTime cardExpiration)
        {
            var orderStartedDomainEvent = new OrderStartedDomainEvent(UserName, cardTypeId, cardNumber, securityNumber, cardHolderName, cardExpiration, this);
            this.AddDomain(orderStartedDomainEvent);
        }

        public void AddOrderItem(int productId, string productName, decimal unitPrice, string pictureUrl, int quantity = 1)
        {
            OrderItem orderItem = new(productId, productName, unitPrice, pictureUrl, quantity);
            _orderItems.Add(orderItem);
        } 
        public void SetBuyerId(Guid buyerId)
        {
            BuyerId = buyerId;
        } 
        public void SetPaymentMethodId(Guid paymentMethodId)
        {
            PaymentMethodId = paymentMethodId;
        }
    }
}

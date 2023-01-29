using MediatR;
using OrderService.Domain.AggregateModels.BuyerAggregate;
using System;

namespace OrderService.Domain.Events
{
    public class BuyerAndPaymentMethodVerifiedDomainEvent : INotification
    {
        public Buyer Buyer { get; }
        public PaymentMethod PaymentMethod { get; }
        public Guid OrderId { get; }

        public BuyerAndPaymentMethodVerifiedDomainEvent(Buyer buyer, PaymentMethod paymentMethod, Guid orderId)
        {
            Buyer = buyer;
            PaymentMethod = paymentMethod;
            OrderId = orderId;
        }
    }
}

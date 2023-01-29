using OrderService.Domain.Events;
using OrderService.Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OrderService.Domain.AggregateModels.BuyerAggregate
{
    public class Buyer : BaseEntity
    {
        public string Name { get; private set; }
        private List<PaymentMethod> _paymentMethods;
        public IReadOnlyCollection<PaymentMethod> PaymentMethods => _paymentMethods.AsReadOnly();
        protected Buyer()
        {
            _paymentMethods = new List<PaymentMethod>();
        }
        public Buyer(string name)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
        }

        public PaymentMethod VerifyOrAddPaymentMethod(int cardType, string alias, string cardNumber, string securityNumber, string cardHolderName, DateTime expire, Guid orderId)
        {
            var existingPaymentMethod = _paymentMethods.SingleOrDefault(x => x.IsEqualTo(cardType, cardNumber, securityNumber, cardHolderName, expire));
            if (existingPaymentMethod != null)
            {
                AddDomain(new BuyerAndPaymentMethodVerifiedDomainEvent(this, existingPaymentMethod, orderId));
            }

            var payment = new PaymentMethod(alias, cardNumber, securityNumber, cardHolderName, expire, cardType);
            _paymentMethods.Add(payment);
            AddDomain(new BuyerAndPaymentMethodVerifiedDomainEvent(this, payment, orderId));
            return payment;
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj) || (obj is Buyer buyer && Id.Equals(buyer.Id) && Name == buyer.Name);
        }
    }
}

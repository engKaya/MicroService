using OrderService.Domain.Exceptions;
using OrderService.Domain.SeedWork;
using System;

namespace OrderService.Domain.AggregateModels.BuyerAggregate
{
    public class PaymentMethod : BaseEntity
    {
        public Guid BuyerId { get; set; }
        public string Alias { get; set; }
        public string CardNumber { get; set; }
        public string SecurityNumber { get; set; }
        public string CardHolderName { get; set; }
        public DateTime CardExpiration { get; set; }
        public int CardTypeId { get; set; }
        public CardType CardType { get; private set; }

        public PaymentMethod(string alias, string cardNumber, string securityNumber, string cardHolderName, DateTime cardExpiration, int cardTypeId)
        {
            Alias = alias;
            CardNumber = !string.IsNullOrWhiteSpace(cardNumber) ? cardNumber : throw new OrderingDomainException(nameof(cardNumber));
            SecurityNumber = !string.IsNullOrWhiteSpace(securityNumber) ? securityNumber : throw new OrderingDomainException(nameof(securityNumber));
            CardHolderName = !string.IsNullOrWhiteSpace(cardHolderName) ? cardHolderName : throw new OrderingDomainException(nameof(cardHolderName));
            if (cardExpiration < DateTime.UtcNow)
            {
                throw new OrderingDomainException(nameof(cardExpiration));
            }
            CardExpiration = cardExpiration;
            CardTypeId = cardTypeId;
            CardType = CardType.FromValue<CardType>(cardTypeId);
        }

        public bool IsEqualTo(int cardTypeId, string cardNumber, string securityNumber, string cardHolderName, DateTime cardExpiration)
        {
            return CardTypeId == cardTypeId
                && CardNumber == cardNumber
                && SecurityNumber == securityNumber
                && CardHolderName == cardHolderName
                && CardExpiration == cardExpiration;
        }
    }
}

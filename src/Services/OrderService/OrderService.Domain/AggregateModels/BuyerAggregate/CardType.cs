using OrderService.Domain.Exceptions;
using OrderService.Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OrderService.Domain.AggregateModels.BuyerAggregate
{
    public class CardType : Enumaration
    {
        public static CardType Amex = new(1, nameof(Amex).ToLowerInvariant());
        public static CardType MasterCard = new(2, nameof(MasterCard).ToLowerInvariant());
        public static CardType Visa = new(3, nameof(Visa).ToLowerInvariant());

        public CardType(int id, string name) : base(id, name)
        {
        }

        public static IEnumerable<CardType> List() => new[] { Amex, MasterCard, Visa };

        public static CardType FromName(string name)
        {
            var state = List().SingleOrDefault(s => string.Equals(s.Name, name, StringComparison.CurrentCultureIgnoreCase));

            return state ?? throw new OrderingDomainException($"Possible values for CardType: {string.Join(",", List().Select(s => s.Name))}");
        }

        public static CardType FromId(int id)
        {
            var state = List().SingleOrDefault(s => s.Id == id);

            return state ?? throw new OrderingDomainException($"Possible values for CardType: {string.Join(" - ", List().Select(s => s.Id.ToString()))}");
        }
    }
}

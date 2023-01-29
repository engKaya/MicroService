using MediatR;
using OrderService.Application.Interfaces.Repositories;
using OrderService.Domain.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OrderService.Application.DomainEventHandlers
{
    public class OrderStartedDomainEventHandler : INotificationHandler<OrderStartedDomainEvent>
    {
        IBuyerRepository _buyerRepository;
        public OrderStartedDomainEventHandler(IBuyerRepository buyerRepository)
        {
            _buyerRepository = buyerRepository;
        }
        public async Task Handle(OrderStartedDomainEvent notification, CancellationToken cancellationToken)
        {
            var cardType = (notification.CardTypeId != 0) ? notification.CardTypeId : 1;
            var buyer = await _buyerRepository.GetSingleAsync(x => x.Name == notification.UserName, x => x.PaymentMethods);

            bool buyerExist = buyer != null;
            if (!buyerExist)
                buyer = new Domain.AggregateModels.BuyerAggregate.Buyer(notification.UserName);

            buyer.VerifyOrAddPaymentMethod(
                    cardType,
                    $"Payment Method On {DateTime.UtcNow}",
                    notification.CardNumber,
                    notification.CardSecurityNumber,
                    notification.CardHolderName,
                    notification.CardExpiration,
                    notification.Order.Id
                    );

            var buyerUpdated = buyerExist ? _buyerRepository.Update(buyer) : await _buyerRepository.AddAsync(buyer);

            await _buyerRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken); 
        }
    }
}

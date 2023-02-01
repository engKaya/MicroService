using EventBus.Base.Abstraction;
using OrderService.Api.IntegrationEvents.Events;
using OrderService.Application.Features.Commands.CreateOrder;
using OrderService.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OrderService.Api.IntegrationEvents.EventHandlers
{
    public class OrderCreatedIntegrationEventHandler : IIntegrationEventHandler<OrderCreatedIntegrationEvent>
    {    
        public Task Handle(OrderCreatedIntegrationEvent @event)
        {
            CreateOrderCommand createOrderCommand = new (
                @event.Basket.Items,
                @event.UserName,
                @event.City,
                @event.Street,
                @event.State,
                @event.Country,
                @event.ZipCode,
                @event.CardNumber,
                @event.CardHolderName,
                @event.CardExpiration,
                @event.CardSecurityNumber,
                @event.CardTypeId
            );
        }
    }
}

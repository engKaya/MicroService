using BasketService.Api.Core.App.Repository;
using BasketService.Api.IntegrationEvents.Events;
using EventBus.Base.Abstraction;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace BasketService.Api.IntegrationEvents.EventHandlers
{
    public class OrderCreatedIntegrationEventHandler : IIntegrationEventHandler<OrderCreatedIntegrationEvent>
    {
        private readonly IBasketRepository basketRepository;
        private readonly ILogger<OrderCreatedIntegrationEventHandler> logger;
        
        public OrderCreatedIntegrationEventHandler(IBasketRepository _basketrepository, ILogger<OrderCreatedIntegrationEventHandler> _logger)
        {
            this.basketRepository = _basketrepository;
            this.logger = _logger;
        }
        public Task Handle(OrderCreatedIntegrationEvent @event)
        {
            logger.LogInformation("----- Handling integration event: {IntegrationEventId} at {AppName} - ({@IntegrationEvent})", @event.Id, GetType().Assembly.GetName().Name , @event);
            return basketRepository.DeleteBasketAsync(@event.BuyerId);
        }
    }
}

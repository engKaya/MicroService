using EventBus.Base.Abstraction;
using Microsoft.Extensions.Logging;
using NotificationService.Api.IntegrationEvents.Events;
using System.Threading.Tasks;

namespace NotificationService.IntegrationEvents.EventHandlers
{
    public class OrderPaymentFailedIntegrationEventHandler : IIntegrationEventHandler<OrderPaymentFailedIntegrationEvent>
    {
        private readonly ILogger<OrderPaymentFailedIntegrationEventHandler> logger;
        public OrderPaymentFailedIntegrationEventHandler(ILogger<OrderPaymentFailedIntegrationEventHandler> _logger)
        {
            logger = _logger;
        }
        public Task Handle(OrderPaymentFailedIntegrationEvent @event)
        {

            // Mail && SMS && Web Push Notification
            logger.LogError($"Order payment failed! OrderId: {@event.OrderId)}. Error Message: {@event.ErrorMessage}");
            return Task.CompletedTask;
        }
    }
}

using EventBus.Base.Abstraction;
using Microsoft.Extensions.Logging;
using NotificationService.Api.IntegrationEvents.Events;
using System.Threading.Tasks;

namespace NotificationService.IntegrationEvents.EventHandlers
{
    public class OrderPaymentSuccessIntegrationEventHandler : IIntegrationEventHandler<OrderPaymentSuccessIntegrationEvent>
    {
        private readonly ILogger<OrderPaymentSuccessIntegrationEventHandler> logger;
        public OrderPaymentSuccessIntegrationEventHandler(ILogger<OrderPaymentSuccessIntegrationEventHandler> _logger)
        {
            logger = _logger;
        }

        public Task Handle(OrderPaymentSuccessIntegrationEvent @event)
        {
            // Mail && SMS && Web Push Notification
            logger.LogWarning($"Order payment success! OrderId: {@event.OrderId.ToString()}");
            return Task.CompletedTask;
        }
    }
}

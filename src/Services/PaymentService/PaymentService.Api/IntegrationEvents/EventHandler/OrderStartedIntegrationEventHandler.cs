using EventBus.Base.Abstraction;
using EventBus.Base.Events;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PaymentService.Api.IntegrationEvents.Events;
using Serilog;
using Serilog.Events;
using System.Threading.Tasks;

namespace PaymentService.Api.IntegrationEvents.EventHandler
{
    public class OrderStartedIntegrationEventHandler : IIntegrationEventHandler<OrderStartedIntegrationEvent>
    {
        private readonly IConfiguration configuration;
        private readonly IEventBus eventBus;
        private readonly ILogger<OrderStartedIntegrationEventHandler> logger;

        public OrderStartedIntegrationEventHandler(IConfiguration configuration, IEventBus eventBus, ILogger<OrderStartedIntegrationEventHandler> logger)
        {
            this.configuration = configuration;
            this.eventBus = eventBus;
            this.logger = logger;
        }

        public Task Handle(OrderStartedIntegrationEvent @event)
        {
            string keyword = "PaymentSuccess";
            bool paymentSuccessfullFlag = configuration.GetValue<bool>(keyword);

            IntegrationEvent paymentEvent = paymentSuccessfullFlag
                    ? new OrderPaymentSuccessIntegrationEvent(@event.OrderId)
                    : new OrderPaymentFailedIntegrationEvent(@event.OrderId, "Error Message");

            logger.LogInformation($"OrderCreatedIntegrationEventHandler in PaymentService is fired with PaymentSuccess: {paymentSuccessfullFlag}, orderId {@event.OrderId}");

            //paymentEvent.CorrelationId = @event.CorrelationId;
            //Log.BindProperty("CorrelationId", @event.CorrelationId, false, out LogEventProperty y);

            eventBus.Publish(paymentEvent);
            return Task.CompletedTask;
        }
    }
}

using EventBus.Base.Events;

namespace PaymentService.Api.IntegrationEvents.Events
{
    public class OrderStartedIntegrationEvent : IntegrationEvent
    {
        public OrderStartedIntegrationEvent()
        {

        }

        public OrderStartedIntegrationEvent(int orderId)
        {
            this.OrderId = orderId;
        }
        public int OrderId { get; set; }
    }
}

using EventBus.Base.Events;

namespace NotificationService.Api.IntegrationEvents.Events
{
    public class OrderPaymentFailedIntegrationEvent : IntegrationEvent
    {
        public int OrderId { get; set; }
        public string ErrorMessage { get; set; }
        public OrderPaymentFailedIntegrationEvent(int OrderId, string ErrMessage)
        {
            this.OrderId = OrderId;
            this.ErrorMessage = ErrMessage;
        }
    }
}

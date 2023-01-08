using EventBus.Base.Events;

namespace PaymentService.Api.IntegrationEvents.Events
{
    public class OrderPaymentFailedIntegrationEvent : IntegrationEvent
    {
        public int OrderId { get; set; }
        public string ErrorMessage { get; set; }
        public OrderPaymentFailedIntegrationEvent(int id, string Message)
        {
            this.OrderId = id;
            this.ErrorMessage = Message;
        }
    }
}

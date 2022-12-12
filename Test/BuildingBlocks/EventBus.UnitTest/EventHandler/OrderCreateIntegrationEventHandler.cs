using EventBus.Base.Abstraction;
using EventBus.UnitTest.Events;
using System.Threading.Tasks;

namespace EventBus.UnitTest.EventHandler
{
    public class OrderCreateIntegrationEventHandler : IIntegrationEventHandler<OrderCreatedIntegrationEvent>
    {
        public Task Handle(OrderCreatedIntegrationEvent @event)
        {
            return Task.CompletedTask;
        }
    }
}

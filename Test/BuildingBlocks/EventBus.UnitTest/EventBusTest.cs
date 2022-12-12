using EventBus.Base;
using EventBus.Base.Abstraction;
using EventBus.Factory;
using EventBus.UnitTest.EventHandler;
using EventBus.UnitTest.Events;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EventBus.UnitTest
{
    [TestClass]
    public class EventBusTest
    {
        private ServiceCollection services;
        public EventBusTest()
        {
            services = new ServiceCollection();
            services.AddLogging(conf => conf.AddConsole());
        }
        [TestMethod]
        public void subscribe_event_on_azure_test()
        {
            services.AddSingleton<IEventBus>(sp =>
            {
                EventBusConfig conf = new()
                {
                    ConnectionRetry = 5,
                    SubscriberClientAppName = "EventBus.UnitTest",
                    DefaultTopicName = "MicroserviceTopicName",
                    EventBusType = EventBusType.AzureServiceBus,
                    EventNameSuffix = "IntegrationEvent",
                    EventBusConnectionString = "Endpoint = sb://ibrahimkaya.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=zyciGoPAgP3uoiXYQqKB39esZLp3Yg0i9QqputI8/mA="
                };

                return EventBusFactory.CreateEventBus(conf, sp);
            });

            var provider = services.BuildServiceProvider();
            var eventBus = provider.GetService<IEventBus>();

            eventBus.Subscribe<OrderCreatedIntegrationEvent, OrderCreateIntegrationEventHandler>();
            eventBus.UnSubscribe<OrderCreatedIntegrationEvent, OrderCreateIntegrationEventHandler>();

        }
    }
}

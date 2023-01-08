using EventBus.Base;
using EventBus.Base.Abstraction;
using EventBus.Factory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NotificationService.Api.IntegrationEvents.Events;
using NotificationService.IntegrationEvents.EventHandlers;
using System;

namespace NotificationService
{
    internal class Program
    {
        static void Main()
        {
            ServiceCollection services = new();
            ConfigureServices(services);

            Console.WriteLine("Notification Service Started!");
            Console.ReadLine();
        }

        

        private static void ConfigureServices(ServiceCollection services)
        {
            services.AddLogging(loggingBuilder =>
            {
                loggingBuilder.AddConsole(); 
            });
            SetTransients(services);
            services.AddSingleton<IEventBus>(sp =>
            {
                EventBusConfig eventBusConfig = new()
                {
                    ConnectionRetry = 5,
                    EventNameSuffix = "IntegrationEvent",
                    SubscriberClientAppName = "NotificationService",
                    EventBusType = EventBusType.RabbitMQ
                };

                return EventBusFactory.CreateEventBus(eventBusConfig, sp);
            });



            IEventBus eventBus = services.BuildServiceProvider().GetRequiredService<IEventBus>();
            SetSubscriptions(eventBus);
        }

        public static void SetSubscriptions(IEventBus eventBus)
        {
            eventBus.Subscribe<OrderPaymentSuccessIntegrationEvent, OrderPaymentSuccessIntegrationEventHandler>();
            eventBus.Subscribe<OrderPaymentFailedIntegrationEvent, OrderPaymentFailedIntegrationEventHandler>();
        }

        public static void SetTransients(IServiceCollection services)
        {
            services.AddTransient<OrderPaymentSuccessIntegrationEventHandler>();
            services.AddTransient<OrderPaymentFailedIntegrationEventHandler>();
        }

    }   
}

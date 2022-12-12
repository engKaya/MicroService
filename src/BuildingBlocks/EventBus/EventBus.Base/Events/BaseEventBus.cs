using EventBus.Base.Abstraction;
using EventBus.Base.SubManagers;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace EventBus.Base.Events
{
    public abstract class BaseEventBus : IEventBus
    {
        public readonly IServiceProvider serviceProvider;
        public readonly IEventBusSubscriptionManager subsManager;

        public EventBusConfig config { get; set; }

        public BaseEventBus(IServiceProvider _serviceProvider, EventBusConfig _config)
        {
            serviceProvider = _serviceProvider;
            config = _config;
            subsManager = new InMemoryEventBusSubscriptionManager(ProcessEventName);
        }

        public virtual string ProcessEventName(string eventName)
        {
            if (config.DeleteEventPrefix)
            {
                eventName = eventName.Replace(config.EventNamePrefix, "");
            }

            if (config.DeleteEventSuffix)
            {
                eventName = eventName.Replace(config.EventNameSuffix, "");
            }

            return eventName;
        }

        public virtual string GetSubName(string eventName)
        {
            return $"{config.SubscriberClientAppName}.{ProcessEventName(eventName)}";
        }

        public virtual void Dispose()
        {
            config = null;
            subsManager.Clear();
        }

        public async Task<bool> ProcessEvent(string eventName, string message)
        {
            eventName = ProcessEventName(eventName);
            var processed = false;

            if (subsManager.HasSubscriptionsForEvent(eventName))
            {
                var subscriptions = subsManager.GetHandlersForEvent(eventName);
                using (var scope = serviceProvider.CreateScope())
                {
                    foreach (var sub in subscriptions)
                    {
                        var handler = serviceProvider.GetService(sub.HandlerType);
                        if (handler == null) continue;
                        var eventType = subsManager.GetEventTypeByName($"{config.EventNamePrefix}{eventName}{config.EventNameSuffix}");
                        var integrationEvent = JsonConvert.DeserializeObject(message, eventType);

                        //if (integrationEvent is IntegrationEvent)
                        //{
                        //    config.CorrelationIdSetter?.Invoke((integrationEvent as IntegrationEvent).Id);
                        //}

                        var concreteType = typeof(IIntegrationEventHandler<>).MakeGenericType(eventType);
                        await (Task)concreteType.GetMethod("Handle").Invoke(handler, new object[] { integrationEvent });
                    }
                }
                processed = true;
            }

            return processed;   
        }

        public abstract void Publish(IntegrationEvent @event);  

        public abstract void Subscribe<T, TH>()
            where T : IntegrationEvent
            where TH : IIntegrationEventHandler<T>;

        public abstract void UnSubscribe<T, TH>()
            where T : IntegrationEvent
            where TH : IIntegrationEventHandler<T>;
    }
}

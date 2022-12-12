﻿
using EventBus.Base;
using EventBus.Base.Abstraction;
using EventBus.AzureServiceBus;
using System;

namespace EventBus.Factory
{
    public static class EventBusFactory
    {
        public static IEventBus CreateEventBus(EventBusConfig config, IServiceProvider serviceProvider)
        {
            return config.EventBusType switch
            {
                EventBusType.AzureServiceBus => new EventBusServiceBus(config, serviceProvider),
                _ => throw new NotImplementedException(),
            };

        }
    }
}

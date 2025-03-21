﻿using System;

namespace EventBus.Base
{
    public  class EventBusConfig
    {
        public int ConnectionRetry { get; set; } = 5;
        public string UserName = "guest";
        public string Password = "guest";
        public string DefaultTopicName { get; set; } = "MicroserviceEventBus";
        public string EventBusConnectionString { get; set; } = String.Empty;
        public string SubscriberClientAppName { get; set; } = String.Empty;
        public string EventNamePrefix { get; set; } = String.Empty;
        public string EventNameSuffix { get; set; } = "IntegrationEvent";
        public EventBusType EventBusType { get; set; } = EventBusType.RabbitMQ; 
        public object Connection { get; set; } 
        public bool DeleteEventPrefix  => !String.IsNullOrEmpty(EventNamePrefix);
        public bool DeleteEventSuffix  => !String.IsNullOrEmpty(EventNameSuffix); 
    }

    public enum EventBusType
    {
        RabbitMQ = 0,
        Kafka = 1
    }
}

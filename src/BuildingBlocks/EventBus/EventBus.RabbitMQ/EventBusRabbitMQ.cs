using EventBus.Base;
using EventBus.Base.Events;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System;

namespace EventBus.RabbitMQ
{
    public class EventBusRabbitMQ : BaseEventBus
    {
        RabbitMQPersistenConnection connection;
        private readonly IConnectionFactory connectionFactory; 
        public EventBusRabbitMQ(IServiceProvider _serviceProvider, EventBusConfig _config) : base(_serviceProvider, _config)
        {
            if (_config.Connection != null)
            {
                var json = JsonConvert.SerializeObject(_config.Connection, new JsonSerializerSettings()
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                });

                connectionFactory = JsonConvert.DeserializeObject<ConnectionFactory>(json);
            }
            else
                connectionFactory = new ConnectionFactory();
            
            connection = new RabbitMQPersistenConnection(connectionFactory);
        }

        public override void Publish(IntegrationEvent @event)
        {
            throw new System.NotImplementedException();
        }

        public override void Subscribe<T, TH>()
        {
            var eventName = typeof(T).Name;
            eventName = ProcessEventName(eventName);
            
            if (!subsManager.HasSubscriptionsForEvent(eventName))
            {

            }
        }

        public override void UnSubscribe<T, TH>()
        {
            throw new System.NotImplementedException();
        }

        private IModel CreateConsumerChannel()
        {
            if (!connection.IsConnected)
            {
                connection.TryConnect();
            }
            var channel = connection.CreateModel();
            channel.ExchangeDeclare(exchange:EventBusConfig.DefaultTo, type: "direct");

        }
    }
}

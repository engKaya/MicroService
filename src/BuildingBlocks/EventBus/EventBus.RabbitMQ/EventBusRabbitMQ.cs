using EventBus.Base;
using EventBus.Base.Events;
using Newtonsoft.Json;
using Polly;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Exceptions;
using System;
using System.Net.Sockets;
using System.Text;

namespace EventBus.RabbitMQ
{
    public class EventBusRabbitMQ : BaseEventBus
    {
        RabbitMQPersistenConnection connection;
        private readonly IConnectionFactory connectionFactory;
        private readonly EventBusConfig eventBusConfig;
        private readonly IModel consumerChannel;
        public EventBusRabbitMQ(IServiceProvider _serviceProvider, EventBusConfig _config) : base(_serviceProvider, _config)
        {
            this.eventBusConfig = _config;
            if (_config.Connection != null)
            {
                var jsonSettings = new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.All,
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                };

                var json = JsonConvert.SerializeObject(_config.Connection, jsonSettings);

                connectionFactory = JsonConvert.DeserializeObject<ConnectionFactory>(json, jsonSettings);
            }
            else
                connectionFactory = new ConnectionFactory();
            
            connection = new RabbitMQPersistenConnection(connectionFactory);
            this.consumerChannel = CreateConsumerChannel();
            subsManager.OnEventRemoved += SubsManager_OnEventRemoved;
        }

        private void SubsManager_OnEventRemoved(object sender, string eventName)
        {
            eventName = ProcessEventName(eventName);
            if (!connection.IsConnected) connection.TryConnect();

            consumerChannel.QueueUnbind(queue: eventName, exchange: eventBusConfig.DefaultTopicName, routingKey: eventName);

            if (subsManager.IsEmpty) consumerChannel.Close(); 
        }

        public override void Publish(IntegrationEvent @event)
        {
            if (!connection.IsConnected) connection.TryConnect();

            var policy = Policy.Handle<BrokerUnreachableException>()
                .Or<SocketException>()
                .WaitAndRetry(eventBusConfig.ConnectionRetry, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)), (ex, time) =>
                {
                });

            var eventName = @event.GetType().Name;
            eventName = ProcessEventName(eventName);

            consumerChannel.ExchangeDeclare(eventBusConfig.DefaultTopicName, "direct"); 

            var message = JsonConvert.SerializeObject(@event);
            var body = Encoding.UTF8.GetBytes(message);


            policy.Execute(() =>
            {
                var properties = consumerChannel.CreateBasicProperties();
                properties.DeliveryMode = 2; // persistent

                //consumerChannel.QueueDeclare(queue: GetSubName(eventName), 
                //    durable: true, 
                //    exclusive: false,
                //    autoDelete: false, 
                //    arguments: null);
                
                //consumerChannel.QueueBind(
                //    queue: GetSubName(eventName),
                //    exchange: eventBusConfig.DefaultTopicName,
                //    routingKey: eventName);

                consumerChannel.BasicPublish(exchange: eventBusConfig.DefaultTopicName,
                                             routingKey: eventName,
                                             mandatory: true,
                                             basicProperties: properties,
                                             body: body);
            });
        }

        public override void Subscribe<T, TH>()
        {
            var eventName = typeof(T).Name;
            eventName = ProcessEventName(eventName);
            
            if (!subsManager.HasSubscriptionsForEvent(eventName))
            {
                if (!connection.IsConnected) connection.TryConnect();

                consumerChannel.QueueDeclare(queue: GetSubName(eventName),
                                             durable: true,
                                             exclusive: false,
                                             autoDelete: false,
                                             arguments: null);
                consumerChannel.QueueBind(queue: GetSubName(eventName),
                                          exchange: eventBusConfig.DefaultTopicName,
                                          routingKey: eventName);

            }

            subsManager.AddSubscription<T, TH>();
            StartBasicConsume(eventName);
        }

        public override void UnSubscribe<T, TH>()
        {
            subsManager.RemoveSubscription<T,TH>();
        }

        private IModel CreateConsumerChannel()
        {
            if (!connection.IsConnected)
            {
                connection.TryConnect();
            }
            var channel = connection.CreateModel();
            channel.ExchangeDeclare(exchange: eventBusConfig.DefaultTopicName, type: "direct");
            return channel;
        }

        private void StartBasicConsume(string eventName)
        {
            if (consumerChannel!=null)
            {
                var consumer = new EventingBasicConsumer(consumerChannel);
                consumer.Received += Consumer_Received;

                consumerChannel.BasicConsume(queue: GetSubName(eventName),
                                             autoAck: false,
                                             consumer: consumer);
            }
        }

        private async void Consumer_Received(object sender, BasicDeliverEventArgs e)
        {
            var eventName = e.RoutingKey;
            var message = System.Text.Encoding.UTF8.GetString(e.Body.ToArray());
            try
            {
                await ProcessEvent(eventName, message);
            }
            catch (Exception ex)
            {  
            }
            consumerChannel.BasicAck(e.DeliveryTag, multiple: false);
        } 
    }
}

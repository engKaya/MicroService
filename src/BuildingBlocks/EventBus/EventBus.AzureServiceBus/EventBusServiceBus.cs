using EventBus.Base;
using EventBus.Base.Events;
using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.ServiceBus.Management;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Text;
using System.Threading.Tasks;

namespace EventBus.AzureServiceBus
{
    public class EventBusServiceBus : BaseEventBus
    {
        private ITopicClient topicClient;
        private ManagementClient managementClient;
        private ILogger logger;
        public EventBusServiceBus(EventBusConfig config, IServiceProvider serviceProvider) : base(serviceProvider, config)
        {
            logger = serviceProvider.GetService(typeof(ILogger<EventBusServiceBus>)) as ILogger<EventBusServiceBus>;
            managementClient = new ManagementClient(config.EventBusConnectionString);
            topicClient = CreateTopicClient();
        }

        private ITopicClient CreateTopicClient()
        {
            if (topicClient == null || topicClient.IsClosedOrClosing)
            {
                topicClient = new TopicClient(config.EventBusConnectionString, config.DefaultTopicName, RetryPolicy.Default);
            }

            if (!managementClient.TopicExistsAsync(config.DefaultTopicName).GetAwaiter().GetResult())
            {
                managementClient.CreateTopicAsync(config.DefaultTopicName).GetAwaiter().GetResult();
            }

            return topicClient;
        }

        public override void Publish(IntegrationEvent @event)
        {
            var eventName = @event.GetType().Name; // Example: OrderCreatedIntegrationEvent
            eventName = ProcessEventName(eventName); // Example: OrderCreated

            var eventStr = JsonConvert.SerializeObject(@event);
            var bodyArr = Encoding.UTF8.GetBytes(eventStr);

            var message = new Message
            {
                MessageId = Guid.NewGuid().ToString(),
                Body = bodyArr,
                Label = eventName
            };

            topicClient.SendAsync(message).GetAwaiter().GetResult();
        }

        public override void Subscribe<T, TH>()
        {
            var eventName = ProcessEventName(typeof(T).Name);
            eventName = ProcessEventName(eventName);
            if (!subsManager.HasSubscriptionsForEvent(eventName))
            {
                var subscriptionClient = CreateSubscritionClientIfDoesntExist(eventName);
                RegisterSubscritionClientMessageHandler(subscriptionClient);
            }
            logger.LogInformation($"Subscribing to event {eventName} with {typeof(TH).Name}");
            subsManager.AddSubscription<T, TH>();
        }

        private void RegisterSubscritionClientMessageHandler(ISubscriptionClient client)
        {
            client.RegisterMessageHandler(
            async (mesage, token) =>
            {
                var eventName = $"{mesage.Label}";
                var messageData = Encoding.UTF8.GetString(mesage.Body);
                if (await ProcessEvent(eventName, messageData))
                {
                    await client.CompleteAsync(mesage.SystemProperties.LockToken);
                }
            },
            new MessageHandlerOptions(ExceptionReceivedHandler) { MaxConcurrentCalls = 10, AutoComplete = false });
        }

        private Task ExceptionReceivedHandler(ExceptionReceivedEventArgs args)
        {
            var ex = args.Exception;
            var context = args.ExceptionReceivedContext;

            logger.LogError(ex, "Error Handling: {ExceptionMessage} - Context {@Exceptioncontext}", ex.Message, context);
            return Task.CompletedTask;
        }

        public override void UnSubscribe<T, TH>()
        {
            var eventName = typeof(T).Name;
            try
            {
                var subClient = CreateSubscriptionClient(eventName);
                subClient
                    .RemoveRuleAsync(eventName)
                    .GetAwaiter()
                    .GetResult();
                    
            }
            catch (MessagingEntityNotFoundException )
            {
                logger.LogWarning($"The messaging entity {eventName} couldn't found");
            }

            logger.LogInformation($"Unsubscribe from {eventName}");
            subsManager.RemoveSubscription<T, TH>();
        }

        private ISubscriptionClient CreateSubscritionClientIfDoesntExist(string eventName)
        {
            var subClient = CreateSubscriptionClient(eventName);
            var exist = managementClient.SubscriptionExistsAsync(config.DefaultTopicName, GetSubName(eventName)).GetAwaiter().GetResult();
            if (!exist)
            {
                managementClient.CreateSubscriptionAsync(config.DefaultTopicName, GetSubName(eventName)).GetAwaiter().GetResult();
                RemoveDefaultRule(subClient);
            }
            CreateRuleIfDoesntExist(eventName, subClient);

            return subClient;
        }



        private void CreateRuleIfDoesntExist(string eventName, ISubscriptionClient client)
        {
            bool ruleExist;

            try
            {
                var rule = managementClient.GetRuleAsync(config.DefaultTopicName, eventName, eventName).GetAwaiter().GetResult();
                ruleExist = rule != null;
            }
            catch (MessagingEntityNotFoundException)
            {
                ruleExist = false;
            }

            if (ruleExist)
            {
                client.AddRuleAsync(new RuleDescription()
                {
                    Filter = new CorrelationFilter() { Label = eventName },
                    Name = eventName
                }).GetAwaiter().GetResult();
            }
        }

        private void RemoveDefaultRule(SubscriptionClient subClient)
        {
            try
            {
                subClient
                    .RemoveRuleAsync(RuleDescription.DefaultRuleName)
                    .GetAwaiter()
                    .GetResult();
            }
            catch (MessagingEntityNotFoundException)
            {
                logger.LogWarning("The Messaging entity {DefaultRuleName} couldn't found!", RuleDescription.DefaultRuleName);
            }
        }

        private SubscriptionClient CreateSubscriptionClient(string eventName)
        {
            return new SubscriptionClient(config.EventBusConnectionString, config.DefaultTopicName, GetSubName(eventName));
        }

        public override void Dispose()
        {
            base.Dispose();
            topicClient.CloseAsync().GetAwaiter().GetResult();
            managementClient.CloseAsync().GetAwaiter().GetResult(); 
            topicClient = null;
            managementClient = null;
        }
    }
}

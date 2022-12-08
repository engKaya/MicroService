﻿using EventBus.Base.Abstraction;
using EventBus.Base.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventBus.Base.SubManagers
{
    public class InMemoryEventBusSubscriptionManager : IEventBusSubscriptionManager
    {
        private readonly Dictionary<string, List<SubscriptionInfo>> _handlers;
        private readonly List<Type> _eventTypes;

        public event EventHandler<string> OnEventRemoved;
        public Func<string, string> eventNameGetter;

        public InMemoryEventBusSubscriptionManager(Func<string, string> eventNameGetter)
        {
            _handlers = new Dictionary<string, List<SubscriptionInfo>>();
            _eventTypes = new List<Type>();
            this.eventNameGetter = eventNameGetter;
        }



        public bool IsEmpty => !_handlers.Keys.Any();
        public void Clear() => _handlers.Clear();
        public void AddSubscription<T, TH>() where T : IntegrationEvent where TH : IIntegrationEventHandler<T>
        {
            var eventName = GetEventKey<T>();

            AddSubscription(typeof(TH), eventName);

            if (_eventTypes.Contains(typeof(T)))
            {
                _eventTypes.Add(typeof(T));
            }
        }

        private void AddSubscription(Type type, string eventName)
        {
            if (!HasSubscriptionsForEvent(eventName))
            {
                _handlers.Add(eventName, new List<SubscriptionInfo>());
            }

            if (_handlers[eventName].Any(s => s.HandlerType == type))
            {
                throw new ArgumentException($"Handler Type {type.Name} alreadyy registered for '{eventName}'", nameof(type));
            }

            _handlers[eventName].Add(SubscriptionInfo.Typed(type));
        }

        private SubscriptionInfo FindSubscriptionToRemove<T, TH>() where T : IntegrationEvent where TH : IIntegrationEventHandler<T>
        {
            var eventName = GetEventKey<T>();
            return FindSubscriptionToRemove(eventName, typeof(TH));
        }
        private SubscriptionInfo FindSubscriptionToRemove(string eventName, Type type)
        {
            if (!HasSubscriptionsForEvent(eventName))
            {
                return null;
            }
            return _handlers[eventName].SingleOrDefault(x => x.HandlerType == type);
        }
        public void RemoveSubscription<T, TH>() where T : IntegrationEvent where TH : IIntegrationEventHandler<T>
        {
            var handlerToRemove = FindSubscriptionToRemove<T, TH>();
            var eventName = GetEventKey<T>();

            RemoveHandler(eventName, handlerToRemove);
        }
        private void RemoveHandler(string eventName, SubscriptionInfo subsToRemove)
        {
            if (subsToRemove!=null)
            {
                _handlers[eventName].Remove(subsToRemove);
                var eventType = _eventTypes.SingleOrDefault(s => s.Name == eventName);
                if (eventType != null)
                {
                    _eventTypes.Remove(eventType);
                }
                RaiseOnEvenRemoved();
            }
        }

        public event EventHandler<string> OnEventRemoved;


        public string GetEventKey<T>()
        {
            string eventName = typeof(T).Name;
            return eventNameGetter(eventName);
        }

        public IEnumerable<SubscriptionInfo> GetHandlersForEvent<T>() where T : IntegrationEvent
        {
            var eventName = GetEventKey<T>();
            return GetHandlersForEvent(eventName);
        }

        public IEnumerable<SubscriptionInfo> GetHandlersForEvent(string eventName) => _handlers[eventName];


        public bool HasSubscriptionsForEvent<T>() where T : IntegrationEvent
        {
            var key = GetEventKey<T>();
            return HasSubscriptionsForEvent(key);
        }

        public bool HasSubscriptionsForEvent(string eventName) => _handlers.ContainsKey(eventName);

        public Type GetEventTypeByName(string type)
        { 

        }
    }
}

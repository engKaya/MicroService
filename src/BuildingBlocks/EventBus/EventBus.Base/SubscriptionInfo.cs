﻿using System;

namespace EventBus.Base
{
    public class SubscriptionInfo
    {
        public SubscriptionInfo(Type _handler)
        {
            this.HandlerType = _handler ?? throw new ArgumentNullException(nameof(_handler));   
        }
        public Type HandlerType { get;}

        public static SubscriptionInfo Typed(Type _handler) {
            return new SubscriptionInfo(_handler);
        }
    }
}

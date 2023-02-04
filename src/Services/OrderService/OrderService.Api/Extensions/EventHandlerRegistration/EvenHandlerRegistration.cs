using BasketService.Api.IntegrationEvents.Events;
using Microsoft.Extensions.DependencyInjection;

namespace OrderService.Api.Extensions.EventHandlerRegistration
{
    public static class EvenHandlerRegistration
    {
        public static IServiceCollection AddEventHandlerRegistration(this IServiceCollection services)
        {
            services.AddTransient<OrderCreatedIntegrationEvent>();
            return services;
        }
    }
}

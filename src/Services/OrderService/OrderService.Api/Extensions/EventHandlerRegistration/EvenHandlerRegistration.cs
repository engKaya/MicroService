using Microsoft.Extensions.DependencyInjection;
using OrderService.Api.IntegrationEvents.EventHandlers;

namespace OrderService.Api.Extensions.EventHandlerRegistration
{
    public static class EvenHandlerRegistration
    {
        public static IServiceCollection AddEventHandlerRegistration(this IServiceCollection services)
        {
            services.AddTransient<OrderCreatedIntegrationEventHandler>();
            return services;
        }
    }
}

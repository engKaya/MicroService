using Blazored.LocalStorage;
using Microsoft.Extensions.DependencyInjection;

namespace WebApp.Extensions
{
    public static class ConfigureCustomServices
    {
        public static IServiceCollection SetCustomServices(this IServiceCollection services)
        {
            services.AddBlazoredLocalStorage();
            return services;
        }
    }
}


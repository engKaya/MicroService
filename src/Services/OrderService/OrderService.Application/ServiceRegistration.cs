using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace OrderService.Application
{
    public static class ServiceRegistration
    {
        public static IServiceCollection AppRegistration(this IServiceCollection services)
        {
            var assemblies = Assembly.GetExecutingAssembly();

            services.AddAutoMapper(assemblies);
            services.AddMediatR(assemblies);
            return services;
        }
    } 
}

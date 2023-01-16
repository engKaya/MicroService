using Consul;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Linq;

namespace CatalogService.Api.Extensions
{
    public static class ConsulRegistiration
    {
        public static IServiceCollection AddConsul(this IServiceCollection services, IConfiguration configuration)
        {
            var consulClient = new ConsulClient(config =>
            {
                var address = configuration["CustomSettings:Consul:Address"];
                if (!string.IsNullOrEmpty(address))
                {
                    config.Address = new Uri(address);
                }
            });
            services.AddSingleton<IConsulClient>(p => consulClient);
            return services;
        }

        public static void RegisterConsul(this IApplicationBuilder app, IConfiguration configuration)
        {
            var consulClient = app.ApplicationServices.GetRequiredService<IConsulClient>();
            var lifetime = app.ApplicationServices.GetRequiredService<IApplicationLifetime>();

            var serviceName =  configuration["CustomSettings:Consul:ServiceName"];

            var features = app.Properties["server.Features"] as FeatureCollection;
            var addresses = features.Get<IServerAddressesFeature>();
            var address = addresses.Addresses.First();

            var uri = new Uri(address);

            var registration = new AgentServiceRegistration()
            {
                ID = $"{serviceName}",
                Name = serviceName,
                Address = $"{uri.Host}",
                Port = uri.Port,
                Tags = new[] { $"urlprefix-/{serviceName}" },
                //Check = new AgentServiceCheck()
                //{
                //    DeregisterCriticalServiceAfter = TimeSpan.FromSeconds(5),
                //    Interval = TimeSpan.FromSeconds(5),
                //    Method = "GET",
                //    HTTP = $"{uri.Host}:{uri.Port}/api/health",
                //    Timeout = TimeSpan.FromSeconds(5)
                //}
            };
            consulClient.Agent.ServiceRegister(registration).Wait();
            lifetime.ApplicationStopping.Register(() =>
            {
                consulClient.Agent.ServiceDeregister(registration.ID).Wait();
            });
        }

    }
}

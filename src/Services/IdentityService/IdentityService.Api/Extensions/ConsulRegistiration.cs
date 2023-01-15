using Consul;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

namespace IdentityService.Api.Extensions
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
                ID = $"{serviceName}_{uri.Host}:{uri.Port}",
                Name = serviceName,
                Address = $"{uri.Scheme}://{uri.Host}",
                Port = uri.Port,
                Tags = new[] { $"urlprefix-/{serviceName}", "JWT", "Auth" },
                //Check = new AgentServiceCheck()
                //{ 
                //    DeregisterCriticalServiceAfter = TimeSpan.FromSeconds(5),
                //    Interval = TimeSpan.FromSeconds(5),
                //    Method = "GET",
                //    HTTP = $"{uri.Scheme}://{uri.Host}:{uri.Port}/health",
                //    Timeout = TimeSpan.FromSeconds(25),
                //    TLSSkipVerify = true,
                //    Notes = $"Health Check to {uri.Scheme}://{uri.Host}:{uri.Port}/health With Get on every 10 seconds"
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

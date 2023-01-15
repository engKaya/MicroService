using Consul;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Net;

namespace PaymentService.Api.Extensions
{
    public static class AppExtensions
    {
        public static IServiceCollection AddConsulConfig(this IServiceCollection services, IConfiguration configuration)
        {
            var consul = new ConsulClient(c => c.Address = new Uri(configuration["CustomSettings:Consul:Address"]));
            services.AddSingleton<IConsulClient, ConsulClient>(p => consul);
            return services;
        }
        public static IApplicationBuilder UseConsul(this IApplicationBuilder app)
        {
            var ip = GetApplicationRunngingHost();
            var consulClient = app.ApplicationServices.GetRequiredService<IConsulClient>();
            var logger = app.ApplicationServices.GetRequiredService<ILoggerFactory>().CreateLogger("AppExtensions");
            var lifetime = app.ApplicationServices.GetRequiredService<IApplicationLifetime>();
            var configuration = app.ApplicationServices.GetRequiredService<IConfiguration>();
            var port = configuration["CustomSettings:Port"];
            var registration = new AgentServiceRegistration()
            {
                ID = $"CatalogService",
                Name = typeof(Program).Assembly.GetName().Name,
                Address = "localhost",
                Port = int.Parse(port),  
                Check = new AgentServiceCheck()
                {
                    DeregisterCriticalServiceAfter = TimeSpan.FromSeconds(5),
                    Interval = TimeSpan.FromSeconds(10),
                    HTTP = $"http://localhost:{port}/api/Health",
                    Timeout = TimeSpan.FromSeconds(5)
                }   
            };

            logger.LogInformation("Registering with Consul");
            consulClient.Agent.ServiceDeregister(registration.ID).ConfigureAwait(true);
            consulClient.Agent.ServiceRegister(registration).ConfigureAwait(true);

            lifetime.ApplicationStopping.Register(() =>
            {
                logger.LogInformation("Unregistering from Consul");
                consulClient.Agent.ServiceDeregister(registration.ID).ConfigureAwait(true);
            });

            return app;
        }


        private static string GetApplicationRunngingHost()
        {
            var host = Dns.GetHostName();
            var ip = Dns.GetHostAddresses(host).FirstOrDefault(x => x.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork);
            return ip.ToString();
        }
        
        
    }
}

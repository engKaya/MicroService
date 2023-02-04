using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OrderService.Api.Extensions;
using OrderService.Infastructure.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderService.Api
{
    public class Program
    { 
        public static void Main(string[] args)
        {
            var config = new ConfigurationBuilder()
                 .SetBasePath(System.IO.Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", false, true)
                .AddEnvironmentVariables()
                .Build();

            var host = BuildWebHost(config, args);
            host.MigrateDbContext<OrderDbContext>((context, services) =>
            {
                var logger = services.GetService<ILogger<OrderDbContext>>();
                var env = services.GetService<IWebHostEnvironment>();

                var dbContextSeeder = new OrderDbContextSeed();
                dbContextSeeder.SeedAsync(context, env, logger).Wait();
            }); 
        }

        static IWebHost BuildWebHost(IConfiguration config, string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseDefaultServiceProvider((context, options) => {
                    options.ValidateOnBuild = false;
                })
                .ConfigureAppConfiguration(i => i.AddConfiguration(config))
                .UseStartup<Startup>()
                .Build();
        
    } 
} 

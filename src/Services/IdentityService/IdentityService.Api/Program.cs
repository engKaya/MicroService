using IdentityService.Api.Extensions;
using IdentityService.Api.Infastructure.Context;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.IO;

namespace IdentityService.Api
{
    public class Program
    {
        public static void Main(string[] args)
        { 
            var hostBuilder = CreateHostBuilder(args);

            hostBuilder.MigrationDbContext<IdentityContext>((context, services) =>
            {
                var env = services.GetService<IWebHostEnvironment>();
                var logger = services.GetService<ILogger<IdentityContextSeed>>();

                new IdentityContextSeed()
                        .SeedAsync(context, env, logger)
                        .Wait();
            });

            hostBuilder.Run();
        }


        static IWebHost CreateHostBuilder(string[] args)
        { 
            return WebHost.CreateDefaultBuilder(args)
                   .UseStartup<Startup>() 
                   .UseContentRoot(Directory.GetCurrentDirectory()) 
                   .Build();
        }
    }
}

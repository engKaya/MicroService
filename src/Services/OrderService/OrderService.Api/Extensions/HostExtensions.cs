using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;    
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Polly;
using System;
using System.Data.SqlClient;

namespace OrderService.Api.Extensions
{
    public static class HostExtensions
    {
        public static IWebHost MigrateDbContext<TContext>(this IWebHost webHost, Action<TContext, IServiceProvider> seeder)
            where TContext : DbContext
        {
            using (var scope = webHost.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var context = services.GetService(typeof(TContext)) as TContext;
                var env = services.GetService(typeof(IHostingEnvironment)) as IHostingEnvironment;
                var logger = services.GetService(typeof(ILogger<TContext>)) as ILogger<TContext>;

                try
                {
                    logger.LogInformation($"Migrating starts on ${typeof(TContext).Name}");
                    var retry = Polly.Policy.Handle<SqlException>()
                        .WaitAndRetry(new TimeSpan[] {
                            TimeSpan.FromSeconds(3),
                            TimeSpan.FromSeconds(5),
                            TimeSpan.FromSeconds(7),
                        });

                    retry.Execute(() =>
                    {
                        InvokeSeeder(seeder, context, services);
                        logger.LogInformation($"Migrated {typeof(TContext).Name}");
                    });


                    return webHost;
                }
                catch (Exception ex)
                {
                    logger.LogError($"En Error Occured on migrate, Context: {typeof(TContext).Name}, Error: {ex.Message}{(ex.InnerException == null ? "" :  $"Inner Ex: {ex.InnerException.Message}")}");
                    return webHost; 
                }
            }
        }

        private static void InvokeSeeder<TContext>(Action<TContext, IServiceProvider> seeder, TContext context, IServiceProvider service) where TContext: DbContext
        {
            context.Database.EnsureCreated();
            context.Database.Migrate();

            seeder(context, service);
        }
    }
}

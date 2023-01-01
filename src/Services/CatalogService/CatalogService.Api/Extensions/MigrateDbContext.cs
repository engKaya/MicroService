using Microsoft.AspNetCore.Hosting;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Polly;
using System;

namespace CatalogService.Api.Extensions
{
    public static class MigrateDbContext
    {
        public static IWebHost MigrationDbContext<TContext>(this IWebHost webHost, Action<TContext, IServiceProvider> seeder) 
            where TContext : DbContext
        {
            using (var scope = webHost.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var logger = services.GetRequiredService<ILogger<TContext>>();
                var context = services.GetRequiredService<TContext>();

                try
                {
                    logger.LogInformation($"Migrating Database associated with context db: {typeof(TContext).Name}");

                    var retry = Policy.Handle<SqlException>()
                            .WaitAndRetry(new TimeSpan[]
                            {
                                TimeSpan.FromSeconds(3),
                                TimeSpan.FromSeconds(5),
                                TimeSpan.FromSeconds(8),
                            });
                    retry.Execute(() => InvokeSeeder(seeder, context, services));

                    logger.LogInformation($"Migrated Database associated with context db: {typeof(TContext).Name}");

                }
                catch (Exception ex)
                {
                    logger.LogError($"An Error Occured While Migrating {typeof(TContext).Name}! Ex: {ex.Message}");
                }

                return webHost;
            }
        }

        private static void InvokeSeeder<TContext>(Action<TContext, IServiceProvider> seeder, TContext context, IServiceProvider service)
            where TContext: DbContext
        {
            context.Database.EnsureCreated();
            context.Database.Migrate();
            seeder(context, service);
        }
    }
}

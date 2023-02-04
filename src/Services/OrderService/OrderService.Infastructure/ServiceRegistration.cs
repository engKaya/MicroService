using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OrderService.Application.Interfaces.Repositories;
using OrderService.Infastructure.Context;
using OrderService.Infastructure.Repositories;

namespace OrderService.Infastructure
{
    public static class ServiceRegistration
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration Configuration)
        {
            services.AddDbContext<OrderDbContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("OrderConnectionString"));
                options.EnableSensitiveDataLogging();
            });
             
            services.AddScoped<IOrderRepository, OrderRepository>(); 
            services.AddScoped<IBuyerRepository, BuyerRepository>();

            var optionsBuilder = new DbContextOptionsBuilder<OrderDbContext>()
                                                .UseSqlServer(Configuration.GetConnectionString("OrderConnectionString"));

            using var dbContext = services.BuildServiceProvider().GetService<OrderDbContext>();
            dbContext.Database.EnsureCreated();
            dbContext.Database.Migrate();
            

            return services;
        } 
    }
}

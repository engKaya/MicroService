using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OrderService.Application.Interfaces.Repositories;
using OrderService.Infastructure.Context;
using OrderService.Infastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

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

            using var dbContext = services.BuildServiceProvider().GetService<OrderDbContext>();
            dbContext.Database.EnsureCreated();
            dbContext.Database.Migrate();
            

            return services;
        } 
    }
}

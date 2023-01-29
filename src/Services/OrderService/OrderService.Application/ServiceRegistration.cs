using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

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

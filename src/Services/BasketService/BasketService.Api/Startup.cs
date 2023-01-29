using BasketService.Api.Core.App.Repository;
using BasketService.Api.Core.App.Services;
using BasketService.Api.Extensions;
using BasketService.Api.Infastructure.Repository;
using BasketService.Api.IntegrationEvents.EventHandlers;
using BasketService.Api.IntegrationEvents.Events;
using EventBus.Base;
using EventBus.Base.Abstraction;
using EventBus.Factory;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace BasketService.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            ConfigureServiceExtensions(services);
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "BasketService.Api", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "BasketService.Api v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.RegisterConsul(Configuration); 
        }


        private void ConfigureServiceExtensions(IServiceCollection services)
        {
            services.ConfigureAuth(Configuration);
            services.AddConsul(Configuration);
            services.AddHttpContextAccessor();
            services.AddSingleton(sp => sp.AddRedis(Configuration));
            services.AddScoped<IBasketRepository, BasketRepository>(); 
            services.AddTransient<IIdentityService, IdentityService>();
            services.AddSingleton<IEventBus>(sp =>
            {
                EventBusConfig config = new EventBusConfig()
                {
                    ConnectionRetry = 5,
                    SubscriberClientAppName = "BasketService",
                    EventBusType = EventBusType.RabbitMQ,
                    EventNameSuffix = "IntegrationEvent",
                };

                return EventBusFactory.CreateEventBus(config, sp);
            });

            SubscribeEvents(services);

        }

        private void SubscribeEvents(IServiceCollection services)
        { 
            var eventBus = services.BuildServiceProvider().GetRequiredService<IEventBus>();
            eventBus.Subscribe<OrderCreatedIntegrationEvent, OrderCreatedIntegrationEventHandler>();
        }

        
    }
}

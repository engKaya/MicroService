using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OrderService.Infastructure.Context
{
    public class OrderDbContextDesignFactory : IDesignTimeDbContextFactory<OrderDbContext>
    {
        public OrderDbContext CreateDbContext(string[] args)
        {
            var conStr = "Data Source=DESKTOP-UUGUHVO;Initial Catalog=ORDER;Persist Security Info=True;User ID=order_app;Password=order_app!!";

            var optionsBuilder = new DbContextOptionsBuilder<OrderDbContext>()
                .UseSqlServer(conStr);

            return new OrderDbContext(optionsBuilder.Options, new NoMediator());
        }
    }

    class NoMediator : IMediator
    {
        public Task Publish(object notification, CancellationToken cancellationToken = default)
        {
            return Task.CompletedTask;
        }

        public Task Publish<TNotification>(TNotification notification, CancellationToken cancellationToken = default) where TNotification : INotification
        {
            return Task.CompletedTask;
        }

        public Task<TResponse> Send<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(default(TResponse));
        }

        public Task<object> Send(object request, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(default(object));
        }
    }


}

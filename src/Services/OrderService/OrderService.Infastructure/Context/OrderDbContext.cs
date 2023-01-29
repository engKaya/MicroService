using MediatR;
using Microsoft.EntityFrameworkCore;
using OrderService.Domain.AggregateModels.BuyerAggregate;
using OrderService.Domain.AggregateModels.OrderAggregate;
using OrderService.Domain.SeedWork;
using OrderService.Infastructure.EntityConfiguration;
using OrderService.Infastructure.Extensions;
using System.Threading;
using System.Threading.Tasks;

namespace OrderService.Infastructure.Context
{
    public class OrderDbContext : DbContext, IUnitOfWork
    {
        public const string DEFAULT_SCHEMA = "ORDER";

        private readonly IMediator mediator;
        public OrderDbContext() : base()
        {

        }

        public OrderDbContext(DbContextOptions<OrderDbContext> options, IMediator mediator) : base(options)
        {
            this.mediator = mediator;
        }


        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<PaymentMethod> PaymentMethods { get; set; }
        public DbSet<Buyer> Buyers { get; set; } 
        public DbSet<CardType> CardTypes { get; set; }
        public DbSet<OrderStatus> OrderStatuses { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new OrderEntityConfiguration());
            modelBuilder.ApplyConfiguration(new BuyerEntityConfiguration());
            modelBuilder.ApplyConfiguration(new CardTypeEntityConfiguration());
            modelBuilder.ApplyConfiguration(new OrderItemsEntityConfiguration());
            modelBuilder.ApplyConfiguration(new OrderStatusEntityConfiguration());
            modelBuilder.ApplyConfiguration(new PaymentMethodEntityConfiguration());
        }

        public async Task<bool> IUnitOfWork.SaveChangesAsync(CancellationToken cancellation)
        {
            await mediator.DispatchDomainEventsAsync(this);
            await base.SaveChangesAsync(cancellation);
            return true;
        }
        public Task<bool> SaveEntitiesAsync(CancellationToken cancellation = default)
        {
            throw new System.NotImplementedException();
        }
    }
}

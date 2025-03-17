using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrderService.Domain.AggregateModels.OrderAggregate;
using OrderService.Infastructure.Context;

namespace OrderService.Infastructure.EntityConfiguration
{
    public class OrderEntityConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.ToTable("ORDERS", OrderDbContext.DEFAULT_SCHEMA);
            builder.HasKey(o => o.Id);
            builder.Ignore(o => o.DomainEvents);
            builder.Property(o => o.Id).ValueGeneratedOnAdd();
            builder
                .OwnsOne(o => o.Address, a =>
                {
                    a.WithOwner();
                });

            builder.Property<int>("orderStatusId")
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .HasColumnName("OrderStatusId")
                .IsRequired();

            var navigation = builder.Metadata.FindNavigation(nameof(Order.OrderItems));
            navigation.SetPropertyAccessMode(PropertyAccessMode.Field);

            builder.HasOne(o => o.Buyer)
                .WithMany()
                .HasForeignKey(x=>x.BuyerId);

            builder.HasOne(x=>x.OrderStatus)
                .WithMany()
                .HasForeignKey("orderStatusId"); 

        }
    }
}

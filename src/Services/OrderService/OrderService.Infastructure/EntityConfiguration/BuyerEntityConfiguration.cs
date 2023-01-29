using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrderService.Domain.AggregateModels.BuyerAggregate;
using OrderService.Infastructure.Context;

namespace OrderService.Infastructure.EntityConfiguration
{
    internal class BuyerEntityConfiguration : IEntityTypeConfiguration<Buyer>
    {
        public void Configure(EntityTypeBuilder<Buyer> builder)
        {
            builder.ToTable("BUYERS", OrderDbContext.DEFAULT_SCHEMA);
            builder.HasKey(x => x.Id);
            builder.Ignore(x => x.DomainEvents);
            builder.Property(x => x.Id).ValueGeneratedOnAdd();
            builder.Property(x => x.Name).HasColumnType("name").HasColumnType("varchar");
            builder.HasMany(x => x.PaymentMethods)
                .WithOne()
                .HasForeignKey(x => x.Id)
                .OnDelete(DeleteBehavior.Cascade);

            var navigate = builder.Metadata.FindNavigation(nameof(Buyer.PaymentMethods));
            navigate.SetPropertyAccessMode(PropertyAccessMode.Field);
        }
    }
}

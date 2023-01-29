using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrderService.Domain.AggregateModels.BuyerAggregate;
using OrderService.Infastructure.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderService.Infastructure.EntityConfiguration
{
    internal class PaymentMethodEntityConfiguration : IEntityTypeConfiguration<PaymentMethod>
    {
        public void Configure(EntityTypeBuilder<PaymentMethod> builder)
        {
            builder.ToTable("PAYMENT_METHODS", OrderDbContext.DEFAULT_SCHEMA);
            builder.Ignore(b => b.DomainEvents);
            builder.HasKey(b => b.Id);
            builder.Property(b => b.Id).HasColumnName("Id").ValueGeneratedOnAdd();
            builder.Property(b => b.Alias).HasColumnName("Alias").HasMaxLength(200).IsRequired();
            builder.Property<int>("BuyerId").IsRequired();
            builder.Property(x => x.CardHolderName)
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .HasColumnName("CardHolderName")
                .HasMaxLength(200)
                .IsRequired();

            builder.Property(x => x.CardNumber)
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .HasColumnName("CardNumber")
                .HasMaxLength(25)
                .IsRequired();

            builder.Property(x => x.CardExpiration)
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .HasColumnName("CardExpiration")
                .HasMaxLength(5)
                .IsRequired();

            builder.Property<int>("CardTypeId")
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .HasColumnName("CardTypeId")
                .IsRequired();

            builder.HasOne(x => x.CardType)
                .WithMany()
                .HasForeignKey("CardTypeId");
        }
    }
}

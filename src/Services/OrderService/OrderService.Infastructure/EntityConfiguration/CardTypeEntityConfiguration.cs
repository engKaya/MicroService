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
    internal class CardTypeEntityConfiguration : IEntityTypeConfiguration<CardType>
    {
        public void Configure(EntityTypeBuilder<CardType> builder)
        {
            builder.ToTable("CARDTYPES", OrderDbContext.DEFAULT_SCHEMA);
            builder.HasKey(c => c.Id);
            builder.Property(c => c.Id).HasColumnName("Id").ValueGeneratedOnAdd();
            builder.Property(c => c.Id)
                .HasDefaultValue(1)
                .ValueGeneratedNever()
                .IsRequired();
            builder.Property(c => c.Name)
                .HasMaxLength(200)
                .IsRequired();
        }
    }
}

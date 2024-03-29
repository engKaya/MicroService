﻿using CatalogService.Api.Core.Domain;
using CatalogService.Api.Infastructure.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CatalogService.Api.Infastructure.EntityConfigurations
{
    public class CatalogTypeEntityTypeConfiguration : IEntityTypeConfiguration<CatalogType>
    {
        public void Configure(EntityTypeBuilder<CatalogType> builder)
        {
            builder.ToTable("CATALOGTYPE");
            builder.HasKey(ci => ci.Id);
            builder.Property(ci => ci.Id)
                .UseHiLo("catalog_type_hilo")
                .IsRequired();
            builder.Property(cb => cb.Type)
                .IsRequired()
                .HasMaxLength(100);
        }
    }
}

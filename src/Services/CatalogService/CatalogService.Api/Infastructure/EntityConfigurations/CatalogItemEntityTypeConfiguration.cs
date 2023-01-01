using CatalogService.Api.Core.Domain;
using CatalogService.Api.Infastructure.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CatalogService.Api.Infastructure.EntityConfigurations
{
    public class CatalogItemEntityTypeConfiguration : IEntityTypeConfiguration<CatalogItem>
    {
        public void Configure(EntityTypeBuilder<CatalogItem> builder)
        {
            builder.ToTable("CATALOG");
            builder.HasKey(ci => ci.Id);
            builder.Property(ci => ci.Id)
                .UseHiLo("catalog_hilo")
                .IsRequired(true);
            builder.Property(cb => cb.Name)
                .IsRequired(true)
                .HasMaxLength(50);
            builder.Property(cb => cb.Price)
                .IsRequired();
            builder.Property(cb => cb.PictureFileName)
                .IsRequired(false);
            builder.Ignore(cb => cb.PictureUri);
            builder.HasOne(ci => ci.CatalogBrand)
                .WithMany()
                .HasForeignKey(ci => ci.CatalogBrandId);
            builder.HasOne(ci => ci.CatalogType)
                .WithMany()
                .HasForeignKey(ci => ci.CatalogTypeId);
        }
    }
}

﻿using CatalogService.Api.Core.Domain;
using CatalogService.Api.Infastructure.EntityConfigurations;
using Microsoft.EntityFrameworkCore;

namespace CatalogService.Api.Infastructure.Context
{
    public class CatalogContext : DbContext
    {
        public const string DEFAULT_SCHEMA = "CATALOG";
        public CatalogContext(DbContextOptions<CatalogContext> options) : base(options)
        {
        }

        public DbSet<CatalogItem> CatalogItems { get; set; }
        public DbSet<CatalogBrand> CatalogBrands { get; set; }
        public DbSet<CatalogType> CatalogTypes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new CatalogItemEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new CatalogBrandEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new CatalogTypeEntityTypeConfiguration());
        }
    }
}

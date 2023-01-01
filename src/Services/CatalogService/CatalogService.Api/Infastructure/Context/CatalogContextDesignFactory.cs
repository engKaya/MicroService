using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;

namespace CatalogService.Api.Infastructure.Context
{
    public class CatalogContextDesignFactory : IDesignTimeDbContextFactory<CatalogContext>
    {
        public CatalogContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<CatalogContext>()
                .UseSqlServer("Data Source=DESKTOP-UUGUHVO;Initial Catalog=CATALOG;Persist Security Info=True;User ID=catalog_app;Password=app");

            return new CatalogContext(optionsBuilder.Options);
        }
    }
}

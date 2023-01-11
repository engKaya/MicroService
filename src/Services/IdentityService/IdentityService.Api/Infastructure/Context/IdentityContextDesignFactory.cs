using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace IdentityService.Api.Infastructure.Context
{
    public class IdentityContextDesignFactory : IDesignTimeDbContextFactory<IdentityContext>
    {
        public IdentityContext CreateDbContext(string[] args)
        {

            var optionsBuilder = new DbContextOptionsBuilder<IdentityContext>()
                .UseSqlServer("Data Source=DESKTOP-UUGUHVO;Initial Catalog=PROFILES;Persist Security Info=True;User ID=profile_app;Password=profile_app2023!!");
            
            return new IdentityContext(optionsBuilder.Options);
        }
    }
}

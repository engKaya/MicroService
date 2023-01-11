using IdentityService.Api.Core.Domain;
using IdentityService.Api.Infastructure.EntityConfigurations;
using Microsoft.EntityFrameworkCore;

namespace IdentityService.Api.Infastructure.Context
{
    public class IdentityContext : DbContext 
    {
        public IdentityContext(DbContextOptions<IdentityContext> contextOptions) : base(contextOptions)
        {
            
        }

        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UserEntityTypeConfiguration());
        }
    }
}

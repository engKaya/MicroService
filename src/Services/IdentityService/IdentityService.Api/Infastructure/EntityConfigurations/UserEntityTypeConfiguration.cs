using IdentityService.Api.Core.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace IdentityService.Api.Infastructure.EntityConfigurations
{
    public class UserEntityTypeConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("USERS");
            builder.HasKey(ci => ci.Id);
            builder.Property(ci => ci.Password)
                .IsRequired();
            builder.Property(ci => ci.Id)
                .UseHiLo("user_hilo")
                .IsRequired();
            builder.Property(cb => cb.Email)
                .IsRequired()
                .HasMaxLength(100);
            builder.Property(cb => cb.FirstName)
                .IsRequired()
                .HasMaxLength(100);
            builder.Property(cb => cb.LastName)
                .IsRequired()
                .HasMaxLength(100);
        }
    }
}

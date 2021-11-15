using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistance.Configuration
{
    public class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            builder.HasMany(p => p.Posts)
                .WithOne(p => p.User)
                .HasForeignKey(p => p.UserId);

            builder.HasMany(p => p.RefreshTokens)
                .WithOne(p => p.User)
                .HasForeignKey(p => p.UserId);
        }
    }
}
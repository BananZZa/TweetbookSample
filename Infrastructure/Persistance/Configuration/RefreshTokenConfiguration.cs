using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistance.Configuration
{
    public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
    {
        public void Configure(EntityTypeBuilder<RefreshToken> builder)
        {
            builder.HasKey(p => p.Token);
            builder.Property(p => p.Token);
            builder.Property(p => p.JwtId);
            builder.Property(p => p.CreationDate);
            builder.Property(p => p.ExpiryDate);
            builder.Property(p => p.Used);
            builder.Property(p => p.Invalidated);
        }
    }
}
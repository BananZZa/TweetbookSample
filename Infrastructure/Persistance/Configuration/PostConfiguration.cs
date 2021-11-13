using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistance.Configuration
{
    public class PostConfiguration : IEntityTypeConfiguration<Post>
    {
        public void Configure(EntityTypeBuilder<Post> builder)
        {
            builder.Property(p => p.Id)
                .IsRequired();
            builder.HasKey(p => p.Id);

            builder.Property(p => p.Content)
                .IsRequired();
            
            builder.Property(p => p.Title)
                .IsRequired();
            
            builder.HasMany(p => p.Tags)
                .WithMany(p => p.Posts);
        }
    }
}
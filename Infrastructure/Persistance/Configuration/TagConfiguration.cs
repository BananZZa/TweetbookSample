using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistance.Configuration
{
    public class TagConfiguration : IEntityTypeConfiguration<Tag>
    {
        public void Configure(EntityTypeBuilder<Tag> builder)
        {
            builder.Property(p => p.TagId)
                .IsRequired();
            builder.HasKey(p => p.TagId);

            builder.HasMany(p => p.Posts)
                .WithMany(p => p.Tags);
        }
    }
}
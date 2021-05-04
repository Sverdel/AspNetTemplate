using AspNetTemplate.Core.Model;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AspNetTemplate.Infrastructure.DataAccess.Configurations
{
    [UsedImplicitly]
    public sealed class EntityConfiguration : IEntityTypeConfiguration<Entity>
    {
        public void Configure(EntityTypeBuilder<Entity> builder)
        {
            builder.HasKey(e => e.Id);
            
            builder.Property(e => e.Id).IsRequired();
            builder.Property(e => e.Name).IsRequired().HasMaxLength(30);
        }
    }
}
using Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations;

public class RoleConfiguration : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.HasKey(e => e.Id);

        builder.Property(e => e.Name);

        builder.HasMany(e => e.Users)
            .WithOne()
            .HasForeignKey(f => f.RoleId);
    }
}
using Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations;

internal class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(e => e.Id);

        builder.Property(e => e.Name);
        // #todo: add unique attribute
        builder.Property(e => e.Phone);
        // #todo: add unique attribute
        builder.Property(e => e.Email);
        builder.Property(e => e.PasswordHash);

        builder.HasOne(e => e.Role)
            .WithMany(m=>m.Users)
            .HasForeignKey(f => f.RoleId);

        builder.HasIndex(p => p.Email)
            .IsUnique();
        
        builder.HasIndex(p => p.Phone)
            .IsUnique();
    }
}
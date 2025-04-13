using Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations;

internal class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(e => e.Id);

        builder.Property(e => e.FullName);
        // #todo: add unique attribute
        builder.Property(e => e.PhoneNumber);
        // #todo: add unique attribute
        builder.Property(e => e.Email);
        builder.Property(e => e.PasswordHash);

        builder.HasOne(e => e.Role)
            .WithMany(m=>m.Users)
            .HasForeignKey(f => f.RoleId);

        builder.HasIndex(p => p.Email)
            .IsUnique();
        
        builder.HasIndex(p => p.PhoneNumber)
            .IsUnique();
        
        builder.HasMany(e => e.RefreshTokens)
            .WithOne(o=>o.User)
            .HasForeignKey(f => f.UserId);
    }
}
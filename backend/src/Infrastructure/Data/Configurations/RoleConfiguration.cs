﻿using Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations;

internal class RoleConfiguration : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.HasKey(e => e.Id);
        // #todo: add unique attribute
        builder.Property(e => e.Name);

        builder.HasMany(e => e.Users)
            .WithOne(o=>o.Role)
            .HasForeignKey(f => f.RoleId);
    }
}
using Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations;

public class WarehouseConfiguration : IEntityTypeConfiguration<Warehouse>
{
    public void Configure(EntityTypeBuilder<Warehouse> builder)
    {
        builder.HasKey(e => e.Id);

        builder.Property(e => e.Name);
        builder.Property(e => e.Location);

        builder.HasMany(e => e.Inventories)
            .WithOne()
            .HasForeignKey(f => f.WarehouseId);
    }
}
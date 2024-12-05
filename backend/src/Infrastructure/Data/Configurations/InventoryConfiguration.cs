using Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations;

public class InventoryConfiguration : IEntityTypeConfiguration<Inventory>
{
    public void Configure(EntityTypeBuilder<Inventory> builder)
    {
        builder.HasKey(e => e.Id);

        builder.Property(e => e.Quantity);

        builder.HasOne(e => e.Product)
            .WithMany()
            .HasForeignKey(f => f.ProductId);

        builder.HasOne(e => e.Warehouse)
            .WithMany(m => m.Inventories)
            .HasForeignKey(f => f.WarehouseId);
    }
}
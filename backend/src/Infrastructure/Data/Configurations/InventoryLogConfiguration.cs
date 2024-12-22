using Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations;

internal class InventoryLogConfiguration : IEntityTypeConfiguration<InventoryLog>
{
    public void Configure(EntityTypeBuilder<InventoryLog> builder)
    {
        builder.HasKey(e => e.Id);

        builder.Property(e => e.Timestamp);
        builder.Property(e => e.QuantityChanged);
        builder.Property(e => e.ChangeType)
            .HasConversion<string>();

        builder.HasOne(e => e.Inventory)
            .WithMany(m=>m.InventoryLogs)
            .HasForeignKey(f => f.InventoryId);
    }
}
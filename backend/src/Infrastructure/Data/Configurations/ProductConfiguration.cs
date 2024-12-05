using Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.HasKey(e => e.Id);

        builder.Property(e => e.Name);
        builder.Property(e => e.SKU);
        builder.Property(e => e.Description);
        builder.Property(e => e.UnitPrice);

        builder.HasOne(e => e.Category)
            .WithMany()
            .HasForeignKey(f => f.CategoryId);

        builder.HasMany(e => e.InventoryLogs)
            .WithOne()
            .HasForeignKey(f => f.ProductId);

        builder.HasIndex(p => p.SKU)
            .IsUnique();
    }
}
using Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations;

internal class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.HasKey(e => e.Id);

        builder.Property(e => e.Name);
        // #todo: add unique attribute
        builder.Property(e => e.SKU);
        builder.Property(e => e.Description);
        builder.Property(e => e.UnitPrice);

        builder.HasOne(e => e.Category)
            .WithMany(m=>m.Products)
            .HasForeignKey(f => f.CategoryId);

        builder.HasMany(e => e.Inventories)
            .WithOne(o=>o.Product)
            .HasForeignKey(f => f.ProductId);

        builder.HasIndex(p => p.SKU)
            .IsUnique();
    }
}
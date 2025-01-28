using Infrastructure.Data;
using Infrastructure.Entities;

namespace API.GraphQL.Queries;

[QueryType]
public sealed class ProductQueries
{
    public IQueryable<Product> GetProducts([Service] InventoryContext context)
    {
        return context.Products.Select(p => new Product
        {
            Id = p.Id,
            Name = p.Name,
            Description = p.Description,
            UnitPrice = p.UnitPrice,
            SKU = p.SKU
        });
    }
}
using Bogus;
using Infrastructure.Entities;

namespace SmartInventoryManagement.Tests.Fakers;

public sealed class WarehouseFaker : Faker<Warehouse>
{
    public WarehouseFaker()
    {
        RuleFor(w => w.Id, f => f.Random.Guid());
        RuleFor(w => w.Name, f => f.Company.CompanyName());
        RuleFor(w => w.Location, f => f.Address.City());
        // RuleFor(w => w.Inventories, f => new List<Inventory>());
    }
}
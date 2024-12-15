using Infrastructure.Data;
using Infrastructure.Interfaces.Repositories.Warehouse;
using Infrastructure.Repositories.Base;

namespace Infrastructure.Repositories.Warehouse;

public class WarehouseRepository : GenericRepository<Entities.Warehouse>, IWarehouseRepository
{
    public WarehouseRepository(InventoryContext context) : base(context)
    {
    }
}
using Infrastructure.Data;
using Infrastructure.Entities;
using Infrastructure.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class RoleRepository : IRoleRepository
{
    private readonly InventoryContext _dbContext;

    public RoleRepository(InventoryContext context)
    {
        _dbContext = context;
    }

    public async Task<Role?> GetByNameAsync(string name)
    {
        return await _dbContext.Roles.FirstOrDefaultAsync(r => r.Name == name);
    }
}
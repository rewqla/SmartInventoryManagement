using Infrastructure.Entities;

namespace Infrastructure.Interfaces.Repositories;

public interface IRoleRepository
{
    Task<Role?> GetByNameAsync(string name);
}
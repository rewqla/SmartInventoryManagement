using Infrastructure.Entities;
using Infrastructure.Interfaces.Repositories.Base;

namespace Infrastructure.Interfaces.Repositories;

public interface IUserRepository : IGenericRepository<User>
{
    Task<User?> GetByEmailOrPhoneAsync(string emailOrPhone);
    Task<User?> GetByIdWithRoles(Guid id);
}
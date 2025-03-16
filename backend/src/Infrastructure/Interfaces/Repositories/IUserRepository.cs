using Infrastructure.Entities;
using Infrastructure.Interfaces.Repositories.Base;

namespace Infrastructure.Interfaces.Repositories;

public interface IUserRepository
{
    Task<User?> GetByEmailOrPhoneAsync(string emailOrPhone);
    Task<User?> GetByIdWithRoles(Guid id);
    Task AddAsync(User user);
}
using Infrastructure.Entities;

namespace Infrastructure.Interfaces;

public interface IUserRepository
{
    Task<User?> GetByEmailOrPhoneAsync(string emailOrPhone);
}
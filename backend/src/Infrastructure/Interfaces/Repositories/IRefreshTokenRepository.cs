using Infrastructure.Entities;

namespace Infrastructure.Interfaces.Repositories;

public interface  IRefreshTokenRepository
{
    Task SaveRefreshTokenAsync(RefreshToken refreshToken);
    Task RemoveOldRefreshTokensAsync(Guid userId);
    Task<RefreshToken?> GetRefreshTokenAsync(string token);
}
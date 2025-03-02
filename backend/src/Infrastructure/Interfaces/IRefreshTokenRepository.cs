using Infrastructure.Entities;

namespace Infrastructure.Interfaces;

public interface  IRefreshTokenRepository
{
    Task SaveRefreshTokenAsync(RefreshToken refreshToken);
    Task RemoveOldRefreshTokensAsync(Guid userId);
    Task<RefreshToken?> GetRefreshTokenAsync(string token);
}
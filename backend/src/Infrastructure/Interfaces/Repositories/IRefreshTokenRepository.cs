using Infrastructure.Entities;

namespace Infrastructure.Interfaces.Repositories;

public interface  IRefreshTokenRepository
{
    Task SaveRefreshTokenAsync(RefreshToken refreshToken);
    Task DeleteByUserIdAsync(Guid userId);
    Task<RefreshToken?> GetRefreshTokenAsync(string token);
    Task DeleteTokensAsync(List<RefreshToken> expiredTokens);
}
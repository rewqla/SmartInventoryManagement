using Coravel.Invocable;
using Infrastructure.Interfaces.Repositories;
using Microsoft.Extensions.Logging;

namespace Application.BackgroundJobs;

public sealed class CleanupExpiredTokensJob(
    IRefreshTokenRepository _refreshTokenRepository,
    ILogger<CleanupExpiredTokensJob> logger): IInvocable
{
    /// <summary>
    /// Executes the cleanup operation for expired user tokens in the database.
    /// </summary>
    public async Task Invoke()
    {
        logger.LogInformation("Starting refresh token cleanup job...");

        var expiredTokens = await _refreshTokenRepository.GetExpiredTokensAsync();
        if (expiredTokens.Count > 0)
        {
            await _refreshTokenRepository.DeleteTokensAsync(expiredTokens);
            logger.LogInformation($"Deleted {expiredTokens.Count} expired refresh tokens.");
        }
        else
        {
            logger.LogInformation("No expired refresh tokens found.");
        }
    }
}

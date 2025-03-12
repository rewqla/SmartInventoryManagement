using Infrastructure.Data;
using Infrastructure.Entities;
using Infrastructure.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class RefreshTokenRepository: IRefreshTokenRepository
{
    private readonly InventoryContext _context;

    public RefreshTokenRepository(InventoryContext context)
    {
        _context = context;
    }

    public async Task SaveRefreshTokenAsync(RefreshToken refreshToken)
    {
        await _context.RefreshTokens.AddAsync(refreshToken);
        
        await _context.SaveChangesAsync();
    }

    public async Task DeleteByUserIdAsync(Guid userId)
    {
        var tokens = _context.RefreshTokens.Where(rt => rt.UserId == userId);
       
        if (tokens.Any())
        {
            _context.RefreshTokens.RemoveRange(tokens);
            await _context.SaveChangesAsync();

        }
    }

    public async Task<RefreshToken?> GetRefreshTokenAsync(string token)
    {
        return await _context.RefreshTokens
            .FirstOrDefaultAsync(rt => rt.Token == token);
    }

    public async Task DeleteTokensAsync(List<RefreshToken> expiredTokens)
    {
        _context.RefreshTokens.RemoveRange(expiredTokens);
        await _context.SaveChangesAsync();
    }
}
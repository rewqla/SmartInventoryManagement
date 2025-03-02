using Infrastructure.Data;
using Infrastructure.Entities;
using Infrastructure.Interfaces;
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

    public async Task RemoveOldRefreshTokensAsync(Guid userId)
    {
        var oldTokens = _context.RefreshTokens.Where(rt => rt.UserId == userId);
        _context.RefreshTokens.RemoveRange(oldTokens);
        
        await _context.SaveChangesAsync();
    }

    public async Task<RefreshToken?> GetRefreshTokenAsync(string token)
    {
        return await _context.RefreshTokens
            .FirstOrDefaultAsync(rt => rt.Token == token);
    }
}
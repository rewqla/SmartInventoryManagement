using Infrastructure.Entities;

namespace Application.Interfaces.Authentication;

public interface ITokenService
{
    string GenerateJwtToken(User user);
    RefreshToken  GenerateRefreshToken(User user);
}
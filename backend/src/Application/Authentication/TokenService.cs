using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Application.Interfaces.Authentication;
using Infrastructure.Entities;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SharedKernel;

namespace Application.Authentication;

public class TokenService : ITokenService
{
    private readonly JwtOptions _options;

    public TokenService(IOptions<JwtOptions> options)
    {
        _options = options.Value ?? throw new ArgumentNullException(nameof(options));
    }

  public string GenerateJwtToken(User user)
    {
        if (string.IsNullOrWhiteSpace(_options.SecretKey) || Encoding.UTF8.GetByteCount(_options.SecretKey) < 32)
        {
            throw new InvalidOperationException("Secret key must be at least 32 bytes long.");
        }

        if (!TimeSpan.TryParse(_options.AccessTokenLifetime, out var accessTokenLifetime))
        {
            throw new InvalidOperationException("AccessTokenLifetime must be a valid TimeSpan.");
        }

        if (accessTokenLifetime <= TimeSpan.Zero)
        {
            throw new ArgumentOutOfRangeException(nameof(_options.AccessTokenLifetime),
                "AccessTokenLifetime must be greater than zero.");
        }

        var expirationTime = DateTime.UtcNow.Add(accessTokenLifetime);
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.SecretKey));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(ClaimTypes.Name, user.Name),
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Role, user.Role.Name),
        };

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = expirationTime,
            Issuer = _options.Issuer ?? null, 
            Audience = _options.Audience ?? null, 
            SigningCredentials = credentials
        };

        var handler = new JwtSecurityTokenHandler();
        var token = handler.CreateToken(tokenDescriptor);

        return handler.WriteToken(token);
    }

    public RefreshToken GenerateRefreshToken(User user)
    {
        if (!TimeSpan.TryParse(_options.RefreshTokenLifetime, out var refreshTokenLifetime))
        {
            throw new FormatException("RefreshTokenLifetime has an invalid format.");
        }

        if (refreshTokenLifetime <= TimeSpan.Zero)
        {
            throw new ArgumentOutOfRangeException(nameof(_options.RefreshTokenLifetime),
                "RefreshTokenLifetime must be greater than zero.");
        }

        return new RefreshToken
        {
            Id = GuidV7.NewGuid(),
            UserId = user.Id,
            Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(32)),
            ExpiresOnUtc = DateTime.UtcNow.Add(refreshTokenLifetime)
        };
    }
}
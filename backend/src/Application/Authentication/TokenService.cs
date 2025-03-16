using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Application.Interfaces.Authentication;
using Infrastructure.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SharedKernel;

namespace Application.Services.Authentication;

public class TokenService : ITokenService
{
    private readonly IConfiguration _configuration;

    public TokenService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string GenerateJwtToken(User user)
    {
        //todo: make issuer, audience optional
        var secretKey = _configuration["Jwt:SecretKey"];
        var issuer = _configuration["Jwt:Issuer"];
        var audience = _configuration["Jwt:Audience"];
        var accessTokenLifetimeStr = _configuration["Jwt:AccessTokenLifetime"];

        if (string.IsNullOrWhiteSpace(secretKey) || Encoding.UTF8.GetByteCount(secretKey) < 32)
        {
            throw new InvalidOperationException("Secret key must be at least 32 bytes long.");
        }

        if (!TimeSpan.TryParse(accessTokenLifetimeStr, out var accessTokenLifetime))
        {
            throw new InvalidOperationException("AccessTokenLifetime must be a valid TimeSpan.");
        }

        if (accessTokenLifetime <= TimeSpan.Zero)
        {
            throw new ArgumentOutOfRangeException(nameof(accessTokenLifetimeStr),
                "AccessTokenLifetime must be greater than zero.");
        }

        var expirationTime = DateTime.UtcNow.Add(accessTokenLifetime);
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
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
            Issuer = issuer,
            Audience = audience,
            SigningCredentials = credentials
        };

        var handler = new JwtSecurityTokenHandler();
        var token = handler.CreateToken(tokenDescriptor);

        return handler.WriteToken(token);
    }

    public RefreshToken GenerateRefreshToken(User user)
    {
        var refreshTokenLifetimeStr = _configuration["Jwt:RefreshTokenLifetime"];

        if (!TimeSpan.TryParse(refreshTokenLifetimeStr, out var refreshTokenLifetime))
        {
            throw new FormatException("RefreshTokenLifetime has an invalid format.");
        }

        if (refreshTokenLifetime <= TimeSpan.Zero)
        {
            throw new ArgumentOutOfRangeException(nameof(refreshTokenLifetime),
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
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace SmartInventoryManagement.IntegrationTests.Helpers;

public class AccessTokenProvider
{
    public static string GenerateToken(string role, Guid? userId = null)
    {
        //todo: get apikey from env variable
        var apiKey = "SuperSecretShibaka123456789012345678901234";
        var issuer = "Shibaka";
        var audience = "Shibakianian";

        var tokenHandler = new JwtSecurityTokenHandler();
        var actualUserId = userId ?? Guid.NewGuid();

        var tokenClaims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, actualUserId.ToString()),
            new Claim(ClaimTypes.NameIdentifier, actualUserId.ToString()),
            new Claim(ClaimTypes.Role, role),
        };

        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(apiKey));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(tokenClaims),
            Expires = DateTime.UtcNow.AddDays(1),
            SigningCredentials = credentials,
            Issuer = issuer,
            Audience = audience
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);
    }
}
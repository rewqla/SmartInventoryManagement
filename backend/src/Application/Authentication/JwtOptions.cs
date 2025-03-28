namespace Application.Authentication;

public class JwtOptions
{
    public string Issuer { get; init; }
    public string Audience { get; init; }
    public string SecretKey { get; init; }
    public string AccessTokenLifetime { get; init; }
    public string RefreshTokenLifetime { get; init; }
}
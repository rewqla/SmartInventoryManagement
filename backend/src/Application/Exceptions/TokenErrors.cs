using Application.Common;

namespace Application.Exceptions;

public static class TokenErrors
{
    public static Error TokenGenerationError(string type, string message) => new(
        "Authentication.TokenGenerationError", $"Failed to generate {type} token: {message}");

    public static Error InvalidOrExpiredRefreshToken() => new(
        "Authentication.InvalidRefreshToken", "Refresh token is invalid or expired.");
}

using Application.Common;

namespace Application.Exceptions;

public static class AuthenticationErrors
{
    public static Error InvalidCredentials() => new Error(
        "Authentication.InvalidCredentials", "Incorrect password");
}
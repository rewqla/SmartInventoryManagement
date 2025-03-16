using Application.Common;

namespace Application.Exceptions;

public static class AuthenticationErrors
{
    public static Error InvalidCredentials() => new Error(
        "Authentication.InvalidCredentials", "Incorrect password");
    
    public static Error UserAlreadyExists() =>
        new Error("Authentication.UserAlreadyExists", "A user with this email ot phone already exists.");
}
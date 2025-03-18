using Application.Common;

namespace Application.Exceptions;

public static class AuthenticationErrors
{
    public static Error InvalidCredentials() => new Error(
        "Authentication.InvalidCredentials", "Incorrect password");
    
    public static Error EmailAlreadyExists() =>
        new Error("Authentication.EmailAlreadyExists", "A user with this email already exists.");

    public static Error PhoneAlreadyExists() =>
        new Error("Authentication.PhoneAlreadyExists", "A user with this phone number already exists.");
}

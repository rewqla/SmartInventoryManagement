using Application.Common;

namespace Application.Exceptions;

public static class AuthenticationErrors
{
    public static Error InvalidCredentials() => new (
        "Authentication.InvalidCredentials", "Incorrect password");

    public static Error EmailAlreadyExists() =>
        new ("Authentication.EmailAlreadyExists", "A user with this email already exists.");

    public static Error PhoneAlreadyExists() =>
        new ("Authentication.PhoneAlreadyExists", "A user with this phone number already exists.");

    public static Error AccountLockedOut() =>
        new (
            "Authentication.AccountLockedOut",
            $"The user account is locked due to multiple failed login attempts. Please try again later."
        );
}

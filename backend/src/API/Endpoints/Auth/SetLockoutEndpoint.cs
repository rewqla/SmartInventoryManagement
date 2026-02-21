using API.Endpoints.Constants;
using Application.Configuration;
using FastEndpoints;

namespace API.Endpoints.Auth;

internal class SetLockoutRequest
{
    public int MaxFailedAttempts { get; set; }
    public int LockoutDurationMinutes { get; set; }
}

internal sealed class SetLockoutEndpoint : Endpoint<SetLockoutRequest, LockoutConfig>
{
    public LockoutConfig settings { get; set; }

    public override void Configure()
    {
        Post(AuthEndpoints.Lockout);
        Description(x => x.WithTags("Auth"));
    }

    //todo: make lockout validation
    //todo: make better lockout set
    public override async Task HandleAsync(SetLockoutRequest setLockoutRequest, CancellationToken cancellationToken)
    {
        settings.MaxFailedAttempts = setLockoutRequest.MaxFailedAttempts;
        settings.LockoutDurationMinutes = setLockoutRequest.LockoutDurationMinutes;

        await SendAsync(settings, cancellation: cancellationToken);
    }
}

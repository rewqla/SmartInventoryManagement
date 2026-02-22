using API.Authorization;
using API.Endpoints.Constants;
using Application.Configuration;
using FastEndpoints;

namespace API.Endpoints.Auth;

internal sealed class GetLockoutEndpoint : EndpointWithoutRequest<LockoutConfig>
{
    public LockoutConfig settings { get; set; }

    public override void Configure()
    {
        Get(AuthEndpoints.Lockout);
        Description(x => x.WithTags("Auth"));
        Roles(PolicyRoles.Admin);
    }

    public override async Task HandleAsync(CancellationToken cancellationToken)
    {
        await SendAsync(settings, cancellation: cancellationToken);
    }
}

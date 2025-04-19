namespace API.Endpoints.Auth;

public static class AuthEndpointsExtension
{
    public static IEndpointRouteBuilder MapAuthEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapSignIn();
        app.MapSignUp();
        app.MapLockout();
        app.MapRefreshToken();

        return app;
    }
}
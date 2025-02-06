using API.Endpoints.Reports;
using API.Endpoints.Warehouse;

namespace API.Endpoints;

public static class EndpointsExtensions
{
    public static IEndpointRouteBuilder MapApiEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapWarehouseEndpoints();
        app.MapReportEndpoints();
        app.MapHealthChecks("/health")
            .WithName("HealthCheck")
            .WithTags("System");

        return app;
    }
}
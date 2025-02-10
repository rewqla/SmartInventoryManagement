using API.Endpoints.Constants;
using API.Extensions;
using Application.Interfaces.Services.Warehouse;
using Microsoft.AspNetCore.Mvc;

namespace API.Endpoints.Reports;

public static class WarehousesReportEndpoint
{
    private const string Name = "WarehousesReport";

    public static IEndpointRouteBuilder MapWarehousesReport(this IEndpointRouteBuilder app)
    {
        app.MapGet(ReportEndpoints.Warehouses,
                async ([FromServices] IWarehouseService warehouseService, CancellationToken cancellationToken) =>
                {
                    var result = await warehouseService.GenerateWarehousesReportAsync(cancellationToken);
                    var fileName = $"WarehousesReport_{DateTime.UtcNow:yyyyMMdd_HHmm}.pdf";

                    return result.Match(
                        onSuccess: value => Results.File(result.Value!, "application/pdf", fileName),
                        onFailure:
                        error => Results.Problem(
                            type: "https://httpstatuses.com/500",
                            title: "Internal Server Error",
                            detail: error.Description,
                            statusCode: StatusCodes.Status500InternalServerError)
                    );
                })
            .WithName(Name)
            .WithTags("Reports");

        return app;
    }
}
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

                    return Results.File(result.Value!, "application/pdf", fileName);
                })
            .WithName(Name)
            .WithTags(EndpointTags.Report);

        return app;
    }
}
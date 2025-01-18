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
                    // todo: change file name and write tests for that
                    
                    return Results.File(result.Value!, "application/pdf", "WarehousesReport.pdf");
                })
            .WithName(Name)
            .WithTags("Reports");

        return app;
    }
}
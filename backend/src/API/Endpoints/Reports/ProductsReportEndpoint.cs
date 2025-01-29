using API.Endpoints.Constants;
using API.Extensions;
using Application.Interfaces.Services.Product;
using Application.Interfaces.Services.Warehouse;
using Microsoft.AspNetCore.Mvc;

namespace API.Endpoints.Reports;

public static class ProductsReportEndpoint
{
    private const string Name = "ProductsReport";

    public static IEndpointRouteBuilder MapProductsReport(this IEndpointRouteBuilder app)
    {
        app.MapGet(ReportEndpoints.Products,
                async ([FromServices] IProductService productService, CancellationToken cancellationToken) =>
                {
                    var result = await productService.GenerateProductReportAsync(cancellationToken);
                    var fileName = $"ProductsReport_{DateTime.UtcNow:yyyyMMdd_HHmm}.pdf";

                    return result.Match(
                        onSuccess: value => Results.File(result.Value!, "application/pdf", fileName),
                        onFailure:
                        error => Results.Problem(title: "Internal Server Error", detail: error.Description,
                            statusCode: 500)
                    );
                })
            .WithName(Name)
            .WithTags("Reports");
        return app;
    }
}
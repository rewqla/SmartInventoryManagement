using API.Endpoints.Auth;
using API.Endpoints.Constants;
using API.Extensions;
using Application.Configuration;
using Application.Interfaces.Services.Product;
using Application.Interfaces.Services.Warehouse;
using FastEndpoints;
using Microsoft.AspNetCore.Mvc;

namespace API.Endpoints.Reports;

// public static class ProductsReportEndpoint
// {
//     private const string Name = "ProductsReport";
//
//     public static IEndpointRouteBuilder MapProductsReport(this IEndpointRouteBuilder app)
//     {
//         app.MapGet(ReportEndpoints.Products,
//                 async ([FromServices] IProductService productService, CancellationToken cancellationToken) =>
//                 {
//                     var result = await productService.GenerateProductReportAsync(cancellationToken);
//                     var fileName = $"ProductsReport_{DateTime.UtcNow:yyyyMMdd_HHmm}.pdf";
//
//                     return Results.File(result.Value!, "application/pdf", fileName);
//                 })
//             .WithName(Name)
//             .WithTags(EndpointTags.Report);
//
//         return app;
//     }
// }


//todo: make reprot be time range
internal sealed class ProductsReportEndpoint : EndpointWithoutRequest
{
    public IProductService productService { get; set; }

    public override void Configure()
    {
        Get(ReportEndpoints.Products);
        Description(x => x.WithTags(EndpointTags.Report));
        AllowAnonymous();
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var result = await productService.GenerateProductReportAsync(ct);
        var fileName = $"ProductsReport_{DateTime.UtcNow:yyyyMMdd_HHmm}.pdf";

        await SendBytesAsync(
            bytes: result.Value!,
            fileName: fileName,
            contentType: "application/pdf",
            cancellation: ct
        );
    }
}

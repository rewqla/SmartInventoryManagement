namespace API.Endpoints.Reports;

public static class ReportEndpointsExtension
{
    public static IEndpointRouteBuilder MapReportEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapWarehousesReport();
        
        return app;
    }
}
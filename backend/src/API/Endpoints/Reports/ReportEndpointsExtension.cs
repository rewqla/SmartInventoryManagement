namespace API.Endpoints.Reports;

public static class ReportEndpointsExtension
{
    public static IEndpointRouteBuilder MapReportEndpoints(this IEndpointRouteBuilder app)
    {
        //todo: move to fast ednpoints
        app.MapWarehousesReport();
        // app.MapProductsReport();

        return app;
    }
}

namespace API.Endpoints.Reports;

public static class ReportEndpointsExtension
{
    public static IEndpointRouteBuilder MapReportEndpoints(this IEndpointRouteBuilder app)
    {
        //todo: move to fast ednpoints
        app.MapWarehousesReport();
        //todo: move to fast ednpoints
        app.MapProductsReport();
        
        return app;
    }
}
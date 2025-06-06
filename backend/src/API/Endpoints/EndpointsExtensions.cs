﻿using API.Endpoints.Auth;
using API.Endpoints.News;
using API.Endpoints.Reports;
using API.Endpoints.Warehouse;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;

namespace API.Endpoints;

public static class EndpointsExtensions
{
    public static IEndpointRouteBuilder MapApiEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapWarehouseEndpoints();
        app.MapReportEndpoints();
        app.MapAuthEndpoints();
        app.MapNewsEndpoints();
        
        app.MapHealthChecks("/health", new HealthCheckOptions
        {
            ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
        });

        return app;
    }
}
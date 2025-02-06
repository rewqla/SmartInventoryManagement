using System.Data;
using Infrastructure.Interfaces;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace API.Health;

internal sealed class DatabaseHealthCheck(IDbConnectionFactory dbConnectionFactory) : IHealthCheck
{
    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context,
        CancellationToken cancellationToken = new CancellationToken())
    {
        try
        {
            using var connection = await dbConnectionFactory.CreateConnectionAsync(cancellationToken);
            using var command = connection.CreateCommand();
            
            command.CommandText = "SELECT 1";
           _ =  command.ExecuteScalar();

            return HealthCheckResult.Healthy();
        }
        catch (Exception e)
        {
            return HealthCheckResult.Unhealthy("Database health check failed", e);
        }
    }
}
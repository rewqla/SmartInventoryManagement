using System.Data;
using Infrastructure.Interfaces;
using Npgsql;

namespace Infrastructure.Data;

public class DbContextFactory : IDbContextFactory
{
    private readonly string _connectionString;

    public DbContextFactory(string connectionString)
    {
        _connectionString = connectionString;
    }

    public async Task<IDbConnection> CreateConnectionAsync(CancellationToken cancellationToken = default)
    {
        var connection = new NpgsqlConnection(_connectionString);
        await connection.OpenAsync(cancellationToken);

        return connection;
    }
}
using System.Data;
using Infrastructure.Data;

namespace Infrastructure.Interfaces;

public interface IDbContextFactory
{
    Task<IDbConnection> CreateConnectionAsync(CancellationToken cancellationToken = default);
}
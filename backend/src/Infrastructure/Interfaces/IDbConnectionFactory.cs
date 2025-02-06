using System.Data;
using Infrastructure.Data;

namespace Infrastructure.Interfaces;

public interface IDbConnectionFactory
{
    Task<IDbConnection> CreateConnectionAsync(CancellationToken cancellationToken = default);
}
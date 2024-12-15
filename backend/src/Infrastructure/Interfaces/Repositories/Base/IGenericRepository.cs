namespace Infrastructure.Interfaces.Repositories.Base;

public interface IGenericRepository<TEntity>
{
    Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<TEntity?> FindByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<TEntity> AddAsync(TEntity entities, CancellationToken cancellationToken = default);
    Task AddRangeAsync(TEntity entity, CancellationToken cancellationToken = default);
    void Delete(TEntity entity);
    TEntity Update(TEntity entity);
    Task<int> CompleteAsync();
    Task DisposeAsync();
    void Attach(TEntity entity);
    void Detach(TEntity entity);
    Task ExecuteSqlRawAsync(string query, params object[] parameters);
}
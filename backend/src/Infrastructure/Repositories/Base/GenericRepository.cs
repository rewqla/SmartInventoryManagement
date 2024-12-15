using Infrastructure.Data;
using Infrastructure.Entities;
using Infrastructure.Interfaces.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.Base;

public abstract  class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : BaseEntity
{      
    private readonly InventoryContext _dbContext;
    private readonly DbSet<TEntity> _dbSet;

    protected GenericRepository(InventoryContext context, DbSet<TEntity> dbSet)
    {
        _dbContext = context;
        _dbSet = dbSet;
    }

    public async Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _dbSet.AsNoTracking().ToListAsync(cancellationToken);
    }
    
    public async Task<TEntity?> FindByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _dbSet.SingleOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public async Task<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken)
    {
        var entityEntry = await _dbSet.AddAsync(entity, cancellationToken);
        return entityEntry.Entity;
    }

    public Task AddRangeAsync(TEntity entities, CancellationToken cancellationToken = default)
    {
        return _dbContext.Set<TEntity>().AddRangeAsync(entities);
    }

    public void Delete(TEntity entity)
    {
        _dbSet.Remove(entity);
    }

    public TEntity Update(TEntity entity)
    {
        var entry = _dbContext.Entry(entity);
        entry.State = EntityState.Modified;
        return entity;
    }

    public async Task<int> CompleteAsync()
    {
        return await _dbContext.SaveChangesAsync();
    }

    public async Task DisposeAsync()
    {
        await _dbContext.DisposeAsync();
        GC.SuppressFinalize(this);
    }

    public void Attach(TEntity entity)
    {
        _dbContext.Set<TEntity>().Attach(entity);
    }

    public void Detach(TEntity entity)
    {
        _dbContext.Entry(entity).State = EntityState.Detached;
    }

    public Task ExecuteSqlRawAsync(string query, params object[] parameters)
    {
        return _dbContext.Database.ExecuteSqlRawAsync(query, parameters);
    }
}
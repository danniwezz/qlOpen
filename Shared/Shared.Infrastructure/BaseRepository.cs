using Microsoft.EntityFrameworkCore;
using Shared.Core;
using System.Linq.Expressions;

namespace Shared.Infrastructure;

public abstract class BaseRepository<T> : IBaseRepository<T> where T : class
{
    protected readonly DbContext _dbContext;

    public BaseRepository(DbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<TProjection>> QueryReadonlyAsync<TProjection>(Expression<Func<T, bool>> filter, Expression<Func<T, TProjection>> projection, CancellationToken cancellationToken)
        => await _dbContext.Set<T>().AsNoTracking().Where(filter).Select(projection).ToListAsync(cancellationToken);

    public async Task<List<T>> QueryAsync(Expression<Func<T, bool>> filter, CancellationToken cancellationToken)
        => await _dbContext.Set<T>().Where(filter).ToListAsync(cancellationToken);

    public async Task<T> SingleAsync(Expression<Func<T, bool>> filter, CancellationToken cancellationToken) =>
         await _dbContext.Set<T>().SingleAsync(filter, cancellationToken: cancellationToken);

    public async Task<T?> SingleOrDefaultAsync(Expression<Func<T, bool>> filter, CancellationToken cancellationToken) =>
         await _dbContext.Set<T>().SingleOrDefaultAsync(filter, cancellationToken: cancellationToken);

    public async Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> filter, CancellationToken cancellationToken)
        => await _dbContext.Set<T>().FirstOrDefaultAsync(filter, cancellationToken);

    public void Remove(T entity) => _dbContext.Set<T>().Remove(entity);
    public void Remove(IEnumerable<T> entities) => _dbContext.Set<T>().RemoveRange(entities);

    public void Add(T entity) => _dbContext.Set<T>().Add(entity);

    public void Add(IEnumerable<T> entities) => _dbContext.Set<T>().AddRange(entities);
}

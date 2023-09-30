using System.Linq.Expressions;

namespace Shared.Core;

public interface IBaseRepository<T>
{
    Task<List<TProjection>> QueryReadonlyAsync<TProjection>(Expression<Func<T, bool>> filter, Expression<Func<T, TProjection>> projection, CancellationToken cancellationToken);
    Task<List<T>> QueryAsync(Expression<Func<T, bool>> filter, CancellationToken cancellationToken);
    Task<T?> SingleOrDefaultAsync(Expression<Func<T, bool>> filter, CancellationToken cancellationToken);
    Task<T> SingleAsync(Expression<Func<T, bool>> filter, CancellationToken cancellationToken);
    Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> filter, CancellationToken cancellationToken);

    void Remove(T entity);
    void Remove(IEnumerable<T> entities);

    void Add(T entity);
    void Add(IEnumerable<T> entities);
 
}

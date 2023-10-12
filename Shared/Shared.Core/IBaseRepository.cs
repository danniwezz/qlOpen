using System.Linq.Expressions;

namespace Shared.Core;

public interface IBaseRepository<T>
{
    Task<List<T>> QueryAsync(Expression<Func<T, bool>> filter, CancellationToken cancellationToken);
    Task<T> SingleAsync(Expression<Func<T, bool>> filter, CancellationToken cancellationToken);
    Task<T?> SingleOrDefaultAsync(Expression<Func<T, bool>> filter, CancellationToken cancellationToken);
    Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> filter, CancellationToken cancellationToken);

    void Add(T entity);
 
}

using ServiceModule.Application;

namespace ServiceModule.Infrastructure;
public class ServiceUnitOfWork : IServiceUnitOfWork
{
    private readonly ServiceDbContext _dbContext;

    public ServiceUnitOfWork(ServiceDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}

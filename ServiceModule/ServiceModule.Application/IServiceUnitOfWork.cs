namespace ServiceModule.Application;
public interface IServiceUnitOfWork
{
    Task SaveChangesAsync(CancellationToken cancellationToken);
}

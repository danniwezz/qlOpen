namespace ServiceModule.Core;
public interface IServiceUnitOfWork
{
	Task SaveChangesAsync(CancellationToken cancellationToken);
}

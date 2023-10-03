namespace CustomerModule.Core;
public interface ICustomerUnitOfWork
{
	Task SaveChangesAsync(CancellationToken cancellationToken);
}

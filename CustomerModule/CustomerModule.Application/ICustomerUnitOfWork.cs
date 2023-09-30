namespace CustomerModule.Application;
public interface ICustomerUnitOfWork
{
	Task SaveChangesAsync(CancellationToken cancellationToken);
}

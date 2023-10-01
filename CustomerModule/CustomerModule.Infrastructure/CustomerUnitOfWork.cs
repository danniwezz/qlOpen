using CustomerModule.Core;

namespace CustomerModule.Infrastructure;
public class CustomerUnitOfWork : ICustomerUnitOfWork
{
	private readonly CustomerDbContext _dbContext;

	public CustomerUnitOfWork(CustomerDbContext dbContext)
    {
		_dbContext = dbContext;
	}
    public Task SaveChangesAsync(CancellationToken cancellationToken)
	{
		return _dbContext.SaveChangesAsync(cancellationToken);
	}
}


using CustomerModule.Application;
using CustomerModule.Infrastructure;
using Shared.Infrastructure;

namespace CustomerModule.Core;
public class CustomerRepository : BaseRepository<Customer>, ICustomerRepository
{
	public CustomerRepository(CustomerDbContext dbContext) : base(dbContext)
	{
	}
}

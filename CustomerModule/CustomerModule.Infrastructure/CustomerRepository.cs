using CustomerModule.Core;
using Microsoft.EntityFrameworkCore;
using Shared.Infrastructure;

namespace CustomerModule.Infrastructure;
public class CustomerRepository : BaseRepository<Customer>, ICustomerRepository
{
	public CustomerRepository(CustomerDbContext dbContext) : base(dbContext)
	{
	}
}

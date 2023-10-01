using ServiceModule.Core;

namespace ServiceModule.Infrastructure;
public class ServiceRepository : Shared.Infrastructure.BaseRepository<Service>, IServiceRepository
{

	public ServiceRepository(ServiceDbContext dbContext) : base(dbContext)
	{
	}
}

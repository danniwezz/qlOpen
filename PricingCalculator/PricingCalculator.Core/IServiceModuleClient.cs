using ServiceModule.Public;

namespace PricingCalculator.Core;
public interface IServiceModuleClient
{
	Task<List<ServiceDto>?> GetServices();
	Task AddService(AddServiceRequestDto request);
}

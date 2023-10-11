using ServiceModule.Public;

namespace PricingCalculator.Core;
public interface IServiceModuleClient
{
	Task<List<ServiceDto>?> GetServices();
	Task<long> AddService(AddServiceRequestDto request);
}

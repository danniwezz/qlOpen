using CustomerModule.Public;

namespace PricingCalculator.Core;

public interface ICustomerModuleClient
{
	Task<CustomerDto?> GetCustomer(long customerId);
}

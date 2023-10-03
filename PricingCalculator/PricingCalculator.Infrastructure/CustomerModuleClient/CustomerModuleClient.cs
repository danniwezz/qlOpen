using CustomerModule.Public;
using Microsoft.Extensions.Options;
using PricingCalculator.Core;
using System.Net.Http.Json;

namespace PricingCalculator.Infrastructure.CustomerModuleClient;
public partial class CustomerModuleClient : ICustomerModuleClient
{
	private readonly IOptions<CustomerModuleClientOptions> _customerModuleClientOptions;
	private readonly HttpClient _httpClient;

	public CustomerModuleClient(IOptions<CustomerModuleClientOptions> customerModuleClientOptions) : base()
	{
		_customerModuleClientOptions = customerModuleClientOptions;
		_httpClient = new HttpClient
		{
			BaseAddress = new Uri(_customerModuleClientOptions.Value.BaseUrl)
		};
	}

	public async Task<CustomerDto> GetCustomer(string customerId)
	{
		return await _httpClient.GetFromJsonAsync<CustomerDto>("/customer");
	}
}

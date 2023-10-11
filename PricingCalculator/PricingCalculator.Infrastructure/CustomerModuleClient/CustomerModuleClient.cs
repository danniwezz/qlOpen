using CustomerModule.Public;
using Microsoft.Extensions.Options;
using PricingCalculator.Core;
using System.Net.Http.Json;

namespace PricingCalculator.Infrastructure.CustomerModuleClient;
public partial class CustomerModuleClient : ICustomerModuleClient
{
	private readonly HttpClient _httpClient;

	public CustomerModuleClient(IOptions<CustomerModuleClientOptions> customerModuleClientOptions) : base()
	{
		_httpClient = new HttpClient
		{
			BaseAddress = new Uri(customerModuleClientOptions.Value.BaseUrl)
		};
	}

	public async Task<CustomerDto?> GetCustomer(long customerId) =>  await _httpClient.GetFromJsonAsync<CustomerDto>($"customer/{customerId}");

	public async Task AddCustomer(CustomerDto customerDto) => await _httpClient.PostAsJsonAsync("customer", customerDto);

}

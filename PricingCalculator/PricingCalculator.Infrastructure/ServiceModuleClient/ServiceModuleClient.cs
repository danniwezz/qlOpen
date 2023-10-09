using Microsoft.Extensions.Options;
using PricingCalculator.Core;
using ServiceModule.Public;
using System.Net.Http.Json;

namespace PricingCalculator.Infrastructure.ServiceModuleClient;
public class ServiceModuleClient : IServiceModuleClient
{
	private readonly IOptions<ServiceModuleClientOptions> _serviceModuleClientOptions;
	private readonly HttpClient _httpClient;

	public ServiceModuleClient(IOptions<ServiceModuleClientOptions> serviceModuleClientOptions) : base()
	{

		_serviceModuleClientOptions = serviceModuleClientOptions;
		_httpClient = new HttpClient
		{
			BaseAddress = new Uri(_serviceModuleClientOptions.Value.BaseUrl)
		};
	}

    public async Task<long> AddService(AddServiceRequestDto request)
	{
		var result = await _httpClient.PostAsJsonAsync<AddServiceRequestDto>("service", request);
		return await result.Content.ReadFromJsonAsync<long>();
	}

	public async Task<List<ServiceDto>?> GetServices()
	{
		return await _httpClient.GetFromJsonAsync<List<ServiceDto>>("service");
	}
}

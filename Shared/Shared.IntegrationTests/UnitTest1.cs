

using ServiceModule.Public;
using Shared.IntegrationTests.Helpers;
using System.Net.Http.Json;

namespace Shared.IntegrationTests;

[Collection("Sequential")]
public class UnitTest1
{
    private HttpClient _customerModuleHttpClient;
    private HttpClient _serviceModuleHttpClient;
    private HttpClient _pricingCalculatorHttpClient;
    public UnitTest1()
    {
        _customerModuleHttpClient = new CustomerModuleApiIntegrationTestBase(new TestWebApplicationFactory<CustomerModule.Api.Program>()).GetHttpClient();
        _serviceModuleHttpClient = new ServiceModuleApiIntegrationTestBase(new TestWebApplicationFactory<ServiceModule.Api.Program>()).GetHttpClient();
        _pricingCalculatorHttpClient = new PricingCalculatorApiIntegrationTestBase(new TestWebApplicationFactory<PricingCalculator.Api.Program>()).GetHttpClient();
    }

    [Fact]
	public void Test1()
	{
        _serviceModuleHttpClient.PostAsJsonAsync<AddServiceRequestDto>("service", new AddServiceRequestDto("Service A", 0.2M, "EUR", 1, 5));
        _serviceModuleHttpClient.PostAsJsonAsync<AddServiceRequestDto>("service", new AddServiceRequestDto("Service B", 0.24M, "EUR", 1, 5));
        _serviceModuleHttpClient.PostAsJsonAsync<AddServiceRequestDto>("service", new AddServiceRequestDto("Service C", 0.4M, "EUR", 1, 7));

	}
}

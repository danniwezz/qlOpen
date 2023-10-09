using PricingCalculator.Api;

namespace Shared.IntegrationTests.Helpers;
public class PricingCalculatorApiIntegrationTestBase: IClassFixture<TestWebApplicationFactory<Program>>
{
	protected readonly TestWebApplicationFactory<Program> _factory;

    public PricingCalculatorApiIntegrationTestBase(TestWebApplicationFactory<Program> factory)
	{
		_factory = factory;
	}
    public HttpClient GetHttpClient()
	{
		return _factory.CreateDefaultClient();
	}
}
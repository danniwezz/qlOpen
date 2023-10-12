using CustomerModule.Public;
using PricingCalculator.Application;
using ServiceModule.Public;
using Shared.IntegrationTests.Helpers;
using Shouldly;
using System.Net.Http.Json;

namespace Shared.IntegrationTests;
public class Test_Case_1 : IAsyncLifetime
{
	private CustomerModuleApiIntegrationTestBase _customerModuleApp;
	private readonly ServiceModuleApiIntegrationTestBase _serviceModuleApp;
	public Test_Case_1()
	{
		_customerModuleApp = new CustomerModuleApiIntegrationTestBase(new TestWebApplicationFactory<CustomerModule.Api.Program>());
		_serviceModuleApp = new ServiceModuleApiIntegrationTestBase(new TestWebApplicationFactory<ServiceModule.Api.Program>());
	}

	public async Task DisposeAsync()
	{
		await _customerModuleApp.DisposeAsync();
		await _serviceModuleApp.DisposeAsync();
	}

	public async Task InitializeAsync()
	{
		await _customerModuleApp.InitializeAsync();
		await _serviceModuleApp.InitializeAsync();
	}

	[Fact]
	public async Task CalculateCosts()
	{
		//Arrange
		var serviceModuleClient = _serviceModuleApp.GetHttpClient();
		var serviceAResponse = await serviceModuleClient.PostAsJsonAsync<AddServiceRequestDto>("service", new AddServiceRequestDto("Service A", 0.2M, "EUR", 1, 5));
		var serviceBResponse = await serviceModuleClient.PostAsJsonAsync<AddServiceRequestDto>("service", new AddServiceRequestDto("Service B", 0.24M, "EUR", 1, 5));
		var serviceACesponse = await serviceModuleClient.PostAsJsonAsync<AddServiceRequestDto>("service", new AddServiceRequestDto("Service C", 0.4M, "EUR", 1, 7));
		var services = await serviceModuleClient.GetFromJsonAsync<List<ServiceDto>>("service");
		IPriceCalculatorService priceCalculatorService = new PriceCalculatorService();

		var serviceA = services!.Single(x => x.Name == "Service A");
		var serviceC = services!.Single(x => x.Name == "Service C");

		var customerRequest = new AddCustomerRequestDto("Customer X", new()
		{
			new()
			{
				ServiceId = serviceA.Id,
				ServiceName = serviceA.Name,
				Price = serviceA.Price,
				Currency = serviceA.Currency,
				ValidFromWeekDayNumber = serviceA.ValidFromWeekDayNumber,
				ValidToWeekDayNumber = serviceA.ValidToWeekDayNumber,
				StartDate = new DateOnly(2019, 09, 20),
				EndDate = null,
				Discounts = new List<DiscountDto>()
			},
			new()
			{
				ServiceId = serviceC.Id,
				ServiceName = serviceC.Name,
				Price = serviceC.Price,
				Currency = serviceC.Currency,
				ValidFromWeekDayNumber = serviceC.ValidFromWeekDayNumber,
				ValidToWeekDayNumber = serviceC.ValidToWeekDayNumber,
				StartDate = new DateOnly(2019, 09, 20),
				EndDate = null,
				Discounts = new List<DiscountDto>
				{
					new DiscountDto
					{
						Percentage = 0.2M,
						ValidFrom = new DateOnly(2019, 09, 22),
						ValidTo = new DateOnly(2019, 09, 24)
					}
				}
			}
		});
		var customerModuleClient = _customerModuleApp.GetHttpClient();
		var addCustomerResponse = await customerModuleClient.PostAsJsonAsync<AddCustomerRequestDto>("customer", customerRequest);
		var customerId = await addCustomerResponse.Content.ReadAsStringAsync();
		var customer = await customerModuleClient.GetFromJsonAsync<CustomerDto>($"customer/{customerId}");

		//Act
		var serviceCost = priceCalculatorService.CalculateCustomerCost(customer!, new DateOnly(2019, 10, 1));

		//Assert
		var totalCost = serviceCost.Sum(x => x.TotalCost);
		totalCost.ShouldBe(4.92M);
	}
}

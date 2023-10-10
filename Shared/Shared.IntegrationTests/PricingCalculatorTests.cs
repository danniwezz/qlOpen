

using CustomerModule.Public;
using PricingCalculator.Application;
using PricingCalculator.Public;
using ServiceModule.Public;
using Shared.IntegrationTests.Helpers;
using System.Net.Http.Json;

namespace Shared.IntegrationTests;

public class PricingCalculatorTests
{
    private HttpClient _customerModuleHttpClient;
    private HttpClient _serviceModuleHttpClient;
    private HttpClient _pricingCalculatorHttpClient;
    public PricingCalculatorTests()
    {
        _customerModuleHttpClient = new CustomerModuleApiIntegrationTestBase(new TestWebApplicationFactory<CustomerModule.Api.Program>()).GetHttpClient();
        _serviceModuleHttpClient = new ServiceModuleApiIntegrationTestBase(new TestWebApplicationFactory<ServiceModule.Api.Program>()).GetHttpClient();
        _pricingCalculatorHttpClient = new PricingCalculatorApiIntegrationTestBase(new TestWebApplicationFactory<PricingCalculator.Api.Program>()).GetHttpClient();
    }

    [Fact]
	public async Task Test_Case_1()
	{
        //Arrange
        var serviceAResponse = await _serviceModuleHttpClient.PostAsJsonAsync<AddServiceRequestDto>("service", new AddServiceRequestDto("Service A", 0.2M, "EUR", 1, 5));
        var serviceBResponse = await _serviceModuleHttpClient.PostAsJsonAsync<AddServiceRequestDto>("service", new AddServiceRequestDto("Service B", 0.24M, "EUR", 1, 5));
        var serviceACesponse = await _serviceModuleHttpClient.PostAsJsonAsync<AddServiceRequestDto>("service", new AddServiceRequestDto("Service C", 0.4M, "EUR", 1, 7));
        var services = await _serviceModuleHttpClient.GetFromJsonAsync<List<ServiceDto>>("service");
        IPriceCalculatorService priceCalculatorService = new PriceCalculatorService();

        var serviceA = services.Single(x => x.Name == "Service A");
        var serviceC = services.Single(x => x.Name == "Service C");

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
                },

            }
        });

        //Act
        var addCustomerResponse = await _customerModuleHttpClient.PostAsJsonAsync<AddCustomerRequestDto>("customer", customerRequest);
        var customerId = await addCustomerResponse.Content.ReadAsStringAsync();
        var customer = await _customerModuleHttpClient.GetFromJsonAsync<CustomerDto>($"customer/{customerId}");
        var serviceCost = priceCalculatorService.CalculateCustomerCost(customer, new DateOnly(2019, 10, 1));

        //Assert
        var totalCost = serviceCost.Sum(x => x.TotalCost);
	}

    [Fact]
    public async Task Test_Case_2()
	{
        //Arrange
        var serviceAResponse = await _serviceModuleHttpClient.PostAsJsonAsync<AddServiceRequestDto>("service", new AddServiceRequestDto("Service A", 0.2M, "EUR", 1, 5));
        var serviceBResponse = await _serviceModuleHttpClient.PostAsJsonAsync<AddServiceRequestDto>("service", new AddServiceRequestDto("Service B", 0.24M, "EUR", 1, 5));
        var serviceACesponse = await _serviceModuleHttpClient.PostAsJsonAsync<AddServiceRequestDto>("service", new AddServiceRequestDto("Service C", 0.4M, "EUR", 1, 7));
        var services = await _serviceModuleHttpClient.GetFromJsonAsync<List<ServiceDto>>("service");
        IPriceCalculatorService priceCalculatorService = new PriceCalculatorService();


        var serviceB = services.Single(x => x.Name == "Service B");
        var serviceC = services.Single(x => x.Name == "Service C");

        var customerRequest = new AddCustomerRequestDto("Customer Y", new()
        {
            new()
            {
                ServiceId = serviceB.Id,
                ServiceName = serviceB.Name,
                Price = serviceB.Price,
                Currency = serviceB.Currency,
                ValidFromWeekDayNumber = serviceB.ValidFromWeekDayNumber,
                ValidToWeekDayNumber = serviceB.ValidToWeekDayNumber,
                StartDate = new DateOnly(2018, 01, 01),
                EndDate = null,
                Discounts = new List<DiscountDto>
                {
                     new DiscountDto
                    {
                        Percentage = 1, //1 <==> 100 %
                        ValidFrom = new DateOnly(2018, 1, 1),
                        ValidTo = new DateOnly(2018, 1, 1).AddDays(200)
                    },
                    new DiscountDto
                    {
                        Percentage = 0.2M,
                        ValidFrom = new DateOnly(2019, 09, 22).AddDays(200),
                        ValidTo = null
                    }
                }
            },
            new()
            {
                ServiceId = serviceC.Id,
                ServiceName = serviceC.Name,
                Price = serviceC.Price,
                Currency = serviceC.Currency,
                ValidFromWeekDayNumber = serviceC.ValidFromWeekDayNumber,
                ValidToWeekDayNumber = serviceC.ValidToWeekDayNumber,
                StartDate = new DateOnly(2018, 01, 01),
                EndDate = null,
                Discounts = new List<DiscountDto>
                {
                    new DiscountDto
                    {
                        Percentage = 1, //1 <==> 100 %
                        ValidFrom = new DateOnly(2018, 1, 1),
                        ValidTo = new DateOnly(2018, 1, 1).AddDays(200)
                    },
                    new DiscountDto
                    {
                        Percentage = 0.2M,
                        ValidFrom = new DateOnly(2019, 09, 22).AddDays(200),
                        ValidTo = null
                    }
                }

            }
        });

        //Act
        var addCustomerResponse = await _customerModuleHttpClient.PostAsJsonAsync<AddCustomerRequestDto>("customer", customerRequest);
        var customerId = await addCustomerResponse.Content.ReadAsStringAsync();
        var customer = await _customerModuleHttpClient.GetFromJsonAsync<CustomerDto>($"customer/{customerId}");
        var serviceCost = priceCalculatorService.CalculateCustomerCost(customer, new DateOnly(2019, 10, 1));

        //Assert
        var totalCost = serviceCost.Sum(x => x.TotalCost);

	}
}

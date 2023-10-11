using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using PricingCalculator.Application;
using PricingCalculator.Core;
using PricingCalculator.Public;

namespace PricingCalculator.Api.WebApi;

public static class PriceCalculatorApi
{
	public static void RegisterPriceCalculatorApi(this RouteGroupBuilder apiGroup)
	{
		var group = apiGroup.MapGroup("price");

		group.MapGet("/{customerId}", GetPriceForCustomer)
			.WithDescription("Gets the accumulated costs up until the provided date for a customer.");
	}

	private static async Task<Results<Ok<List<ServiceCostDto>>, UnprocessableEntity<string>>> GetPriceForCustomer(
		ICustomerModuleClient customerModuleClient, 
		IServiceModuleClient serviceModuleClient, 
		IPriceCalculatorService priceCalculatorService,
		[FromRoute] long customerId,
		[FromQuery] DateOnly? calculateUntilDate)
	{
		var customer = await customerModuleClient.GetCustomer(customerId);
		if(customer == null)
		{
			return TypedResults.UnprocessableEntity($"Could not find customer with id: {customerId}.");
		}
		var services = await serviceModuleClient.GetServices();
		if(services == null)
		{
			return TypedResults.UnprocessableEntity($"Could not find get services.");
		}
		if(!services.Any(service => customer.AssignedServices.Select(x => x.ServiceId).Contains(service.Id)))
		{
			return TypedResults.UnprocessableEntity("All supplied assigned services for customer must exist.");
		}

		var serviceCost = priceCalculatorService.CalculateCustomerCost(customer, calculateUntilDate);
		return TypedResults.Ok(serviceCost.Select(x => new ServiceCostDto(x.ServiceName, x.TotalCost, x.Currency)).ToList());
	}
}

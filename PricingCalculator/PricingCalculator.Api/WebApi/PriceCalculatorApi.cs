using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using PricingCalculator.Application;
using PricingCalculator.Core;

namespace PricingCalculator.Api.WebApi;

public static class PriceCalculatorApi
{
	public static void RegisterPriceCalculatorApi(this RouteGroupBuilder apiGroup)
	{
		var group = apiGroup.MapGroup("price");

		group.MapGet("/{customerId}", GetPriceForCustomer);
	}

	private static async Task<Results<Ok<List<ServiceCost>>, UnprocessableEntity<string>>> GetPriceForCustomer(
		ICustomerModuleClient customerModuleClient, 
		IPriceCalculatorService priceCalculatorService,
		[FromRoute] long customerId)
	{
		var customer = await customerModuleClient.GetCustomer(customerId);
		if(customer == null)
		{
			return TypedResults.UnprocessableEntity($"Could not find customer with id: {customerId}.");
		}
		var serviceCost = priceCalculatorService.CalculateCustomerCost(customer);
		return TypedResults.Ok(serviceCost);
	}
}

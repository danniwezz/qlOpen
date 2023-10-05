using CustomerModule.Public;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using PricingCalculator.Core;

namespace PricingCalculator.Api.WebApi;

public static class PriceCalculatorApi
{
	public static void RegisterPriceCalculatorApi(this RouteGroupBuilder apiGroup)
	{
		var group = apiGroup.MapGroup("price");

		group.MapGet("/{customerId}", GetPriceForCustomer);
	}

	private static async Task<Ok<CustomerDto>> GetPriceForCustomer(
		ICustomerModuleClient customerModuleClient, 
		[FromRoute] long customerId)
	{
		var customer = await customerModuleClient.GetCustomer(customerId);
		//TODO Calculate price
		throw new NotImplementedException();
	}
}

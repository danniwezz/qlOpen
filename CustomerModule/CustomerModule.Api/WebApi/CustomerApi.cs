using CustomerModule.Application.Customers;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;

namespace CustomerModule.Api.WebApi;

public static class CustomerApi
{

	public static void RegisterICustomerApi(this RouteGroupBuilder apiGroup)
	{
		var group = apiGroup.MapGroup("customer");

		group.MapPost("", AddCustomer)
			.WithDescription("Adds a customer");
	}

	private static async Task<Ok> AddCustomer(
		IMediator mediator,
		AddCustomer.Request request)
	{
		await mediator.Send(request);
		return TypedResults.Ok();
	}
}

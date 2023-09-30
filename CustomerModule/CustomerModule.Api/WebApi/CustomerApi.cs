using CustomerModule.Application.Customers;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace CustomerModule.Api.WebApi;

public static class CustomerApi
{

	public static void RegisterICustomerApi(this RouteGroupBuilder apiGroup)
	{
		var group = apiGroup.MapGroup("customer");
		group.MapPost("", AddCustomer)
			.WithDescription("Adds a customer");

		group.MapPost("{customerId}/assignedService{serviceId}", AddAssignedService)
			.WithDescription("Adds an assigned service to the specified customer");
	}

	private static async Task<Ok> AddCustomer(
		IMediator mediator,
		AddCustomer.Request request)
	{
		await mediator.Send(request);
		return TypedResults.Ok();
	}

	private static Task AddAssignedService(
		IMediator mediator,
		[FromRoute] long customerId,
		[FromRoute] long serviceId)
	{
		throw new NotImplementedException();
	}
}

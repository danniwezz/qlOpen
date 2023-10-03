using CustomerModule.Application.Customers;
using CustomerModule.Core;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Shared.Core;
using Shared.FluentValidation;

namespace CustomerModule.Api.WebApi;

public static class CustomerApi
{

	public static void RegisterICustomerApi(this RouteGroupBuilder apiGroup)
	{
		var group = apiGroup.MapGroup("customer");

		group.MapPost("", AddCustomer)
			.WithDescription("Adds a customer");
	}

	private static async Task<Results<Ok, UnprocessableEntity<IEnumerable<IError>>>> AddCustomer(
		IMediator mediator,
		AddCustomer.Request request, ICustomerRepository customerRepository)
	{
		var validationResult = await new AddCustomer.Validator(customerRepository).ValidateAsync(request);
		if (!validationResult.IsValid)
		{
			return TypedResults.UnprocessableEntity(validationResult.ToFailureResult());
		}
		await mediator.Send(request);
		return TypedResults.Ok();
	}
}

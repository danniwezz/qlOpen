using CustomerModule.Api.Mappers;
using CustomerModule.Application.Customers;
using CustomerModule.Core;
using CustomerModule.Public;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
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
		group.MapGet("/{customerId}", GetCustomer);
	}

	private static async Task<Results<Ok, UnprocessableEntity<IEnumerable<IError>>>> AddCustomer(
		IMediator mediator,
		[FromBody] AddCustomerRequestDto requestDto,
		ICustomerRepository customerRepository)
	{
		var request = CustomerRequstMapper.FromDto(requestDto);
		var validationResult = await new AddCustomer.Validator(customerRepository).ValidateAsync(request);
		if (!validationResult.IsValid)
		{
			return TypedResults.UnprocessableEntity(validationResult.ToFailureResult());
		}
		await mediator.Send(request);
		return TypedResults.Ok();
	}

	private static async Task<Ok<CustomerDto>> GetCustomer(
		IMediator mediator,
		[FromRoute] long customerId,
		ICustomerRepository customerRepository)
	{
		return TypedResults.Ok(await mediator.Send(new GetCustomer.Request(customerId)));
	}
}

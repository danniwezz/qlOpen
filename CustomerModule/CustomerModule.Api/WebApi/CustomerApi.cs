﻿using CustomerModule.Api.Mappers;
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

	private static async Task<Results<Ok<long>, UnprocessableEntity<IEnumerable<IError>>>> AddCustomer(
		IMediator mediator,
		[FromBody] AddCustomerRequestDto requestDto,
		ICustomerRepository customerRepository)
	{
		var request = CustomerRequestMapper.FromDto(requestDto);
		var validationResult = await new AddCustomer.Validator(customerRepository).ValidateAsync(request);
		if (!validationResult.IsValid)
		{
			return TypedResults.UnprocessableEntity(validationResult.ToFailureResult());
		}
		var customerId = await mediator.Send(request);
		return TypedResults.Ok(customerId);
	}

	private static async Task<Results<Ok<CustomerDto>, UnprocessableEntity<string>>> GetCustomer(
		IMediator mediator,
		[FromRoute] long customerId,
		ICustomerRepository customerRepository)
	{
		var result = await mediator.Send(new GetCustomer.Request(customerId));
		if (result == null)
		{
			return TypedResults.UnprocessableEntity($"Could not find customer with Id: {customerId}");
		}
		return TypedResults.Ok(result);
	}
}

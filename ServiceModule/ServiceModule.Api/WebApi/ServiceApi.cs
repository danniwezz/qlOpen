using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using ServiceModule.Application.Service;
using ServiceModule.Core;
using ServiceModule.Public;
using Shared.Core;
using Shared.FluentValidation;

namespace ServiceModule.Api.WebApi;

public static class ServiceApi
{
	public static void RegisterServiceApi(this RouteGroupBuilder apiGroup)
	{
		var group = apiGroup.MapGroup("service").WithOpenApi();

		group.MapPost("", AddService)
			.WithDescription("Registers a service.");
		group.MapGet("", GetServices)
			.WithDescription("Gets all registered services.");
	}

	private static async Task<Results<Ok<long>, UnprocessableEntity<IEnumerable<IError>>>> AddService(IMediator mediator, AddServiceRequestDto requestDto, IServiceRepository serviceRepository)
	{
		var request = new AddService.Request(requestDto.Name, requestDto.Price, requestDto.Currency, requestDto.ValidFromWeekDayNumber, requestDto.ValidToWeekDayNumber);
		var validator = new AddService.Validator(serviceRepository);
		var validationResult = await validator.ValidateAsync(request);
		if (!validationResult.IsValid)
		{
			return TypedResults.UnprocessableEntity(validationResult.ToFailureResult());
		}
		var serviceId = await mediator.Send(request);
		return TypedResults.Ok(serviceId);
	}

	private static async Task<Ok<List<Service>>> GetServices(IMediator mediator)
	{
		var services = await mediator.Send(new GetServices.Request());
		return TypedResults.Ok(services);
	}
}

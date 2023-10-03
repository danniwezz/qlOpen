using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using ServiceModule.Application.Service;
using ServiceModule.Core;
using Shared.Core;
using Shared.FluentValidation;

namespace ServiceModule.Api.WebApi;

public static class ServiceApi
{
	public static void RegisterServiceApi(this RouteGroupBuilder apiGroup)
	{
		var group = apiGroup.MapGroup("service");

		group.MapPost("", AddService);
		group.MapGet("", GetServices);
	}

	private static async Task<Results<Ok, UnprocessableEntity<IEnumerable<IError>>>> AddService(IMediator mediator, AddService.Request request, IServiceRepository serviceRepository)
	{
		var validator = new AddService.Validator(serviceRepository);
		var validationResult = await validator.ValidateAsync(request);
		if (!validationResult.IsValid)
		{
			return TypedResults.UnprocessableEntity(validationResult.ToFailureResult());
		}
		await mediator.Send(request);
		return TypedResults.Ok();
	}

	private static async Task<Ok<List<ServiceModule.Core.Service>>> GetServices(IMediator mediator)
	{
		var services = await mediator.Send(new GetServices.Request());
		return TypedResults.Ok(services);
	}
}

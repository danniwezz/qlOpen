
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using ServiceModule.Application.Service;

namespace ServiceModule.Api.WebApi;

public static class ServiceApi
{
	public static void RegisterServiceApi(this RouteGroupBuilder apiGroup)
	{
		var group = apiGroup.MapGroup("service");

		group.MapPost("", AddService);
		group.MapGet("", GetServices);
	}

	private static async Task<Ok> AddService(Mediator mediator, AddService.Request request)
	{
		await mediator.Send(request);
		return TypedResults.Ok();
	}

	private static async Task<Ok<List<ServiceModule.Core.Service>>> GetServices(Mediator mediator)
	{
		var services = await mediator.Send(new GetServices.Request());
		return TypedResults.Ok(services);
	}
}

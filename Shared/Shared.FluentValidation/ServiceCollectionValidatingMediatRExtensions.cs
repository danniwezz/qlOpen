using CustomerModule.Application.Shared;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Shared.FluentValidation;
public static class ServiceCollectionValidatingMediatRExtensions
{
	public static IServiceCollection AddValidatingMediator(this IServiceCollection services, Type mediatorType)
	{
		services.AddMediatR(c => c.RegisterServicesFromAssemblyContaining(mediatorType));
		services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidatingBehaviour<,>));
		services.AddValidatorsFromAssemblyContaining<ApplicationValidationException>(ServiceLifetime.Scoped);
		return services;
	}
}

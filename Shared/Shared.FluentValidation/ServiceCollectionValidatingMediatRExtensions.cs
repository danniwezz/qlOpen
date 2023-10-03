using Microsoft.Extensions.DependencyInjection;

namespace Shared.FluentValidation;
public static class ServiceCollectionValidatingMediatRExtensions
{
	public static IServiceCollection AddValidatingMediator(this IServiceCollection services, Type mediatorType)
	{
		services.AddMediatR(c => c.RegisterServicesFromAssemblyContaining(mediatorType));
		return services;
	}
}

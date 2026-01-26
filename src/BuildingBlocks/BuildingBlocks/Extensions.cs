using Mediator;
using Microsoft.Extensions.DependencyInjection;

namespace BuildingBlocks;

public static class Extensions
{
	public static IServiceCollection AddMediatorService(this IServiceCollection services, MediatorOptions? options = null)
	{
		if (options is not null)
		{
			services.AddMediator(o => o = options);

			return services;
		}

		services.AddMediator(opt => opt.ServiceLifetime = ServiceLifetime.Scoped);
		return services;
	}
}
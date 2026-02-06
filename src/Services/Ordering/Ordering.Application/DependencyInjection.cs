using System.Reflection;
using BuildingBlocks;
using BuildingBlocks.Messaging.MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.FeatureManagement;

namespace Ordering.Application;

public static class DependencyInjection
{
	public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
	{
		services.AddMediatorService(new()
		{
			ServiceLifetime = ServiceLifetime.Scoped,
			Assemblies = [Assembly.GetExecutingAssembly()],
			PipelineBehaviors = [typeof(LoggingBehavior<,>)]
		});

		services.AddFeatureManagement();

		services.AddMessageBroker(configuration, Assembly.GetExecutingAssembly());

		return services;
	}
}
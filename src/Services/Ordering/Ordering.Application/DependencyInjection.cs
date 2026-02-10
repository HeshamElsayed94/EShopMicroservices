using System.Reflection;
using BuildingBlocks.Messaging.MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.FeatureManagement;

namespace Ordering.Application;

public static class DependencyInjection
{
	public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
	{
		services.AddMediator(opt =>
		{
			opt.ServiceLifetime = ServiceLifetime.Scoped;
			opt.PipelineBehaviors = [typeof(LoggingBehavior<,>)];
		});

		services.AddFeatureManagement();

		services.AddMessageBroker(configuration, Assembly.GetExecutingAssembly());

		return services;
	}
}
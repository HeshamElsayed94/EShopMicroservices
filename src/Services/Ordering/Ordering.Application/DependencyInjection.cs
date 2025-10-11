namespace Ordering.Application;

public static class DependencyInjection
{

    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddMediator(opt =>
        {
            opt.ServiceLifetime = ServiceLifetime.Scoped;
            opt.PipelineBehaviors = [typeof(LoggingBehavior<,>)];
        });
        return services;
    }

}
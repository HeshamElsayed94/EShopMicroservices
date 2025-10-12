namespace Ordering.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(
       this IServiceCollection services,
       IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("Database")!;

        services.AddScoped<AuditableEntityInterceptor>();
        services.AddScoped<DispatchDomainEventInterceptor>();

        services.AddDbContext<ApplicationDbContext>((sp, opt) =>
        {
            opt.UseSqlServer(connectionString);
            opt.AddInterceptors(sp.GetRequiredService<AuditableEntityInterceptor>(),
                sp.GetRequiredService<DispatchDomainEventInterceptor>());
            opt.EnableSensitiveDataLogging(true);
        });

        return services;
    }
}
namespace Ordering.API;

public static class DependencyInjection
{

    public static WebApplicationBuilder AddApiServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddProblemDetails(options => options.CustomizeProblemDetails = context =>
        {
            context.ProblemDetails.Instance = $"{context.HttpContext.Request.Method} {context.HttpContext.Request.Path}";
            context.ProblemDetails.Extensions.Add("requestId", context.HttpContext.TraceIdentifier);
        });

        builder.Services.Configure<RouteHandlerOptions>(config => config.ThrowOnBadRequest = true);

        builder.Services.Configure<JsonOptions>(config =>
        {
            config.SerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
            config.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
            config.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        });

        builder.Services.AddValidatorsFromAssembly(typeof(Program).Assembly);

        builder.Services.AddFluentValidationAutoValidation(config =>
            config.OverrideDefaultResultFactoryWith<CustomValidationResultFactory>());

        builder.Services.AddExceptionHandler<ExceptionHandler>();

        builder.Services.AddCarter();

        return builder;
    }

    public static WebApplication UseApiServices(this WebApplication app)
    {

        app.MapGroup("").AddFluentValidationAutoValidation().MapCarter();

        return app;
    }
}
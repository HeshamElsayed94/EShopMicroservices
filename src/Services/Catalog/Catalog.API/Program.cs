var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMediator(opt =>
{
    opt.ServiceLifetime = ServiceLifetime.Scoped;
    opt.PipelineBehaviors = [typeof(LoggingBehavior<,>)];
});

builder.Services.AddCarter(assemblyCatalog: new(typeof(Program).Assembly));
builder.Services.AddMarten(opts => opts.Connection(builder.Configuration.GetConnectionString("Database")!))
.UseLightweightSessions();

if (builder.Environment.IsDevelopment())
{
    builder.Services.InitializeMartenWith<CatalogInitialData>();
}

builder.Services.AddProblemDetails(options => options.CustomizeProblemDetails = context =>
{
    context.ProblemDetails.Instance = $"{context.HttpContext.Request.Method} {context.HttpContext.Request.Path}";
    context.ProblemDetails.Extensions.Add("requestId", context.HttpContext.TraceIdentifier);
});

builder.Services.Configure<RouteHandlerOptions>(options => options.ThrowOnBadRequest = true);

builder.Services.AddValidatorsFromAssembly(typeof(Program).Assembly);

builder.Services.AddFluentValidationAutoValidation(config
    => config.OverrideDefaultResultFactoryWith<CustomValidationResultFactory>());

builder.Services.AddHealthChecks()
    .AddNpgSql(builder.Configuration.GetConnectionString("Database")!);

builder.Services.AddExceptionHandler<ExceptionHandler>();

var app = builder.Build();

app.UseExceptionHandler();

app.MapGroup("").AddFluentValidationAutoValidation().MapCarter();

app.UseHealthChecks("/health", new HealthCheckOptions
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

app.Run();
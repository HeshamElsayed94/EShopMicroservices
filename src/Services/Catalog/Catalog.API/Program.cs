using System.Reflection;
using BuildingBlocks;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMediatorService(new()
{
	ServiceLifetime = ServiceLifetime.Scoped,
	Assemblies = [Assembly.GetExecutingAssembly()],
	PipelineBehaviors = [typeof(LoggingBehavior<,>)]
});

builder.Services.AddCarter(new(typeof(Program).Assembly));

builder.Services.AddMarten(opts =>
	{
		opts.Connection(builder.Configuration.GetConnectionString("Database")!);

		opts.CreateDatabasesForTenants(c =>
		{
			c.MaintenanceDatabase(builder.Configuration.GetConnectionString("MaintenanceDatabase")!);

			c.ForTenant()
				.CheckAgainstPgDatabase();
		});
	})
	.UseLightweightSessions()
	.ApplyAllDatabaseChangesOnStartup();

if (builder.Environment.IsDevelopment()) builder.Services.InitializeMartenWith<CatalogInitialData>();

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

builder.Services.Configure<JsonOptions>(config =>
{
	config.SerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
	config.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
	config.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});

var app = builder.Build();

app.UseExceptionHandler();

app.MapGroup("").AddFluentValidationAutoValidation().MapCarter();

app.UseHealthChecks("/health", new HealthCheckOptions
{
	ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

app.Run();
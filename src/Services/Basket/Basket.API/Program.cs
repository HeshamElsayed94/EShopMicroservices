using Discount.Grpc;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddProblemDetails(options => options.CustomizeProblemDetails = context =>
{
	context.ProblemDetails.Instance = $"{context.HttpContext.Request.Method} {context.HttpContext.Request.Path}";
	context.ProblemDetails.Extensions.Add("requestId", context.HttpContext.TraceIdentifier);
});

builder.Services.AddExceptionHandler<ExceptionHandler>();

builder.Services.AddCarter();

builder.Services.AddMediator(opt =>
{
	opt.ServiceLifetime = ServiceLifetime.Scoped;
	opt.PipelineBehaviors = [typeof(LoggingBehavior<,>)];
});

builder.Services.AddMarten(opts =>
{
	opts.Connection(builder.Configuration.GetConnectionString("Database")!);

	opts.Schema.For<ShoppingCart>().Identity(x => x.UserName);

	opts.CreateDatabasesForTenants(c =>
	{
		c.MaintenanceDatabase(builder.Configuration.GetConnectionString("MaintenanceDatabase")!);
		c.ForTenant()
		 .CheckAgainstPgDatabase();
	});

}).UseLightweightSessions()
.ApplyAllDatabaseChangesOnStartup();

builder.Services.Configure<RouteHandlerOptions>(config => config.ThrowOnBadRequest = true);

builder.Services.AddValidatorsFromAssembly(typeof(Program).Assembly);

builder.Services.AddFluentValidationAutoValidation(config =>
	config.OverrideDefaultResultFactoryWith<CustomValidationResultFactory>());

builder.Services.Configure<JsonOptions>(config =>
{
	config.SerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
	config.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
	config.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});

builder.Services.AddHealthChecks()
	.AddNpgSql(builder.Configuration.GetConnectionString("Database")!)
	.AddRedis(builder.Configuration.GetConnectionString("Redis")!);

builder.Services.AddStackExchangeRedisCache(opt =>
{
	opt.InstanceName = "Basket";
	opt.Configuration = builder.Configuration.GetConnectionString("Redis");
});

builder.Services.AddScoped<IBasketRepository, BasketRepository>();
builder.Services.Decorate<IBasketRepository, CachedBasketRepository>();

//Grpc services
builder.Services.AddGrpcClient<DiscountProtoService.DiscountProtoServiceClient>(opt =>
	opt.Address = new(builder.Configuration["GrpcSettings:DiscountUrl"]!)
	)
	.ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
	{
		ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
	});

var app = builder.Build();

app.UseExceptionHandler();

app.MapGroup("").AddFluentValidationAutoValidation().MapCarter();

app.UseHealthChecks("/health", options: new()
{
	ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

app.Run();
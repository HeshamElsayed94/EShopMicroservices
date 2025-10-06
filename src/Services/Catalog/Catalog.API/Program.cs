using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMediator(opt => opt.ServiceLifetime = ServiceLifetime.Scoped);
builder.Services.AddCarter(assemblyCatalog: new(typeof(Program).Assembly));
builder.Services.AddMarten(opts => opts.Connection(builder.Configuration.GetConnectionString("Database")!))
.UseLightweightSessions();

builder.Services.AddProblemDetails(options => options.CustomizeProblemDetails = context =>
{
    context.ProblemDetails.Instance = $"{context.HttpContext.Request.Method} {context.HttpContext.Request.Path}";
    context.ProblemDetails.Extensions.Add("requestId", context.HttpContext.TraceIdentifier);
});

builder.Services.AddControllers()
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
                options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            });

var app = builder.Build();

app.MapCarter();

app.Run();
using System.Text.Json.Serialization;
using BuildingBlocks.Behaviors;
using BuildingBlocks.Exceptions;
using Carter;
using FluentValidation;
using Microsoft.AspNetCore.Http.Json;
using SharpGrip.FluentValidation.AutoValidation.Endpoints.Extensions;

var builder = WebApplication.CreateBuilder(args);

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

var app = builder.Build();

app.UseExceptionHandler();

app.MapGroup("").AddFluentValidationAutoValidation().MapCarter();

app.Run();
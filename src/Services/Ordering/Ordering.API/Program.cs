using Ordering.API;
using Ordering.Application;
using Ordering.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplicationServices()
	.AddInfrastructureServices(builder.Configuration);

builder.AddApiServices();

var app = builder.Build();

app.UseExceptionHandler();

app.UseApiServices();

app.Run();
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplicationServices()
	.AddInfrastructureServices(builder.Configuration);

builder.AddApiServices();

var app = builder.Build();

app.UseExceptionHandler();

app.UseApiServices();

if (app.Environment.IsDevelopment())
{
	await app.InitializeDatabaseAsync();
}

app.Run();
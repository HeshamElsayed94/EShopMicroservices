var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplicationServices(builder.Configuration)
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
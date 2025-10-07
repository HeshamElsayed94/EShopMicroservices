using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace BuildingBlocks.Exceptions;

public class ExceptionHandler(
	ILogger<ExceptionHandler> logger,
	IProblemDetailsService problemDetails,
	IHostEnvironment env) : IExceptionHandler
{

	public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
	{

		logger.LogError(exception, exception.Message);

		httpContext.Response.StatusCode = exception switch
		{
			BadHttpRequestException => StatusCodes.Status400BadRequest,
			_ => StatusCodes.Status500InternalServerError
		};

		return await problemDetails.TryWriteAsync(new()
		{
			HttpContext = httpContext,
			Exception = exception,
			ProblemDetails = new()
			{
				Type = exception.GetType().Name,
				Title = "Error occurred",
				Detail = env.IsDevelopment() ? $"{exception.Message} \n{exception.StackTrace}" : exception.Message
			}
		});

	}
}
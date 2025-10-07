using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using SharpGrip.FluentValidation.AutoValidation.Endpoints.Results;

namespace BuildingBlocks.Behaviors;

public class CustomValidationResultFactory : IFluentValidationAutoValidationResultFactory
{
	public IResult CreateResult(EndpointFilterInvocationContext context, ValidationResult validationResult)
	{
		return Results.ValidationProblem(validationResult.ToDictionary(), statusCode: StatusCodes.Status422UnprocessableEntity);
	}
}
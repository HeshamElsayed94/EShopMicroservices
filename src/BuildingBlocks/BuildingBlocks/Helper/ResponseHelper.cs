using System.Net;
using BuildingBlocks.Common.Results.Errors;
using Microsoft.AspNetCore.Http;

namespace BuildingBlocks.Helper;

public static class ResponseHelper
{

	public static IResult Problem(List<Error> errors)
	{
		if (errors.Count is 0)
			return Results.Problem();

		if (errors.All(err => err.StatusCode == HttpStatusCode.UnprocessableEntity))
			return ValidationProblem(errors);

		return Problem(errors[0]);
	}

	private static IResult ValidationProblem(List<Error> errors)
	{

		var errorsDic = errors.GroupBy(e => e.Code)
			.ToDictionary(g => g.Key, g => g.Select(e => e.Description).ToArray());

		return Results.ValidationProblem(errors: errorsDic, statusCode: StatusCodes.Status422UnprocessableEntity);
	}

	private static IResult Problem(Error error)
		=> Results.Problem(detail: error.Description, statusCode: (int)error.StatusCode);
}
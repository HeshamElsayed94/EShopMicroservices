namespace Catalog.API.Helper;

public static class ResponseHelper
{

	public static IResult Problem(List<Error> errors)
	{
		if (errors.Count is 0)
			return Results.Problem();

		return Problem(errors[0]);
	}

	private static IResult Problem(Error error)
		=> Results.Problem(detail: error.Description, statusCode: (int)error.StatusCode);
}
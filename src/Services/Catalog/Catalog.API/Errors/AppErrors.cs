using BuildingBlocks.Common.Results.Errors;

namespace Catalog.API.Errors;

public static class AppErrors
{
	public static Error ProductNotFound(Guid id) => Error.NotFound(description: $"Product with id '{id}' not found.");
}
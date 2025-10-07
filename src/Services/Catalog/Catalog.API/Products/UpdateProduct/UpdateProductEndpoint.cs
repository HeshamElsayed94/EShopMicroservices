namespace Catalog.API.Products.UpdateProduct;

public record UpdateProductRequest(
    Guid Id,
    string Name,
    List<string> Category,
    string Description,
    string ImageFile,
    decimal Price);

public class UpdateProductCommandValidator : AbstractValidator<UpdateProductRequest>
{
	public UpdateProductCommandValidator()
	{
		RuleFor(x => x.Id).NotEmpty();
		RuleFor(x => x.Name).NotEmpty()
		.Length(2, 150);
		RuleFor(x => x.Category).NotEmpty();
		RuleFor(x => x.ImageFile).NotEmpty();
		RuleFor(x => x.Price).GreaterThanOrEqualTo(0);
	}
}

//public record UpdateProductResponse();

public class UpdateProductEndpoint : ICarterModule
{
	public void AddRoutes(IEndpointRouteBuilder app)
	{
		app.MapPut("/products", async (UpdateProductRequest request, ISender sender, CancellationToken ct) =>
		{
			var command = request.Adapt<UpdateProductCommand>();

			var result = await sender.Send(command, ct);

			return result.Match(_ => Results.NoContent(), ResponseHelper.Problem);

		})
		.WithName("UpdateProduct")
		.Produces(StatusCodes.Status204NoContent)
		.ProducesProblem(StatusCodes.Status422UnprocessableEntity)
		.ProducesProblem(StatusCodes.Status400BadRequest)
		.ProducesProblem(StatusCodes.Status404NotFound)
		.WithDescription("Update Product")
		.WithSummary("Update Product");
	}
}
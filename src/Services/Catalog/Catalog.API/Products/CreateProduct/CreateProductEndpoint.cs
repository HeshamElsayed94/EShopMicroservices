namespace Catalog.API.Products.CreateProduct;

public record CreateProductRequest(
    string Name,
    List<string> Category,
    string Description,
    string ImageFile,
    decimal Price);

public class CreateProductCommandValidator : AbstractValidator<CreateProductRequest>
{
	public CreateProductCommandValidator()
	{
		RuleFor(x => x.Name).NotEmpty();
		RuleFor(x => x.Category).NotEmpty();
		RuleFor(x => x.ImageFile).NotEmpty();
		RuleFor(x => x.Price).GreaterThanOrEqualTo(0);
	}
}

public record CreateProductResponse(Guid Id);

public class CreateProductEndpoint : ICarterModule
{
	public void AddRoutes(IEndpointRouteBuilder app)
	{
		app.MapPost("/products",
		async (CreateProductRequest request, ISender sender, CancellationToken ct) =>
				{
					var command = request.Adapt<CreateProductCommand>();

					var result = await sender.Send(command, ct);

					var response = result.Adapt<CreateProductResponse>();

					return Results.Created($"/products/{response.Id}", response);
				})
				.WithName("CreateProduct")
				.Produces<CreateProductResponse>(StatusCodes.Status201Created)
				.ProducesProblem(StatusCodes.Status400BadRequest)
				.ProducesProblem(StatusCodes.Status422UnprocessableEntity)
				.WithSummary("Create Product")
				.WithDescription("Create Product");
	}
}
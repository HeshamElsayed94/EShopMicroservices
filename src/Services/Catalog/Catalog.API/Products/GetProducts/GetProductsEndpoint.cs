namespace Catalog.API.Products.GetProducts;

public record GetProductsRequest(int? PageNumber = 1, int? PageSize = 10);

public class GetProductsRequestValidator : AbstractValidator<GetProductsRequest>
{
	public GetProductsRequestValidator()
	{
		RuleFor(x => x.PageNumber).GreaterThanOrEqualTo(1);
		RuleFor(x => x.PageSize).InclusiveBetween(1, 50);
	}
}

public record GetProductsResponse(PagedResult<Product> Products);

public class GetProductsEndpoint : ICarterModule
{
	public void AddRoutes(IEndpointRouteBuilder app)
	{
		app.MapGet("/products", async ([AsParameters] GetProductsRequest request, ISender sender, CancellationToken ct) =>
		{
			var query = request.Adapt<GetProductsQuery>();

			var result = await sender.Send(query, ct);

			var response = new GetProductsResponse(result.Products);

			return Results.Ok(response);
		})
		.WithName("GetProducts")
		.Produces<GetProductsResponse>(StatusCodes.Status200OK)
		.ProducesProblem(StatusCodes.Status400BadRequest)
		.WithDescription("Get Products")
		.WithSummary("Get Products");
	}
}
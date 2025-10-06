namespace Catalog.API.Products.GetProductById;

//public record GetProductByIdRequest(Guid Id);
public record GetProductByIdResponse(Product Product);

public class GetProductByIdEndpoint : ICarterModule
{
	public void AddRoutes(IEndpointRouteBuilder app)
	{

		app.MapGet("/products/{id}", async (Guid id, ISender sender, CancellationToken ct) =>
		{
			var result = await sender.Send(new GetProductByIdQuery(id), ct);

			return result.Match(res => Results.Ok(res.Adapt<GetProductByIdResponse>()),
			err => ResponseHelper.Problem(err));

		})
		.WithName("GetProductById")
		.Produces<GetProductByIdResponse>(StatusCodes.Status200OK)
		.ProducesProblem(StatusCodes.Status404NotFound)
		.ProducesProblem(StatusCodes.Status400BadRequest)
		.WithDescription("Get Product By Id")
		.WithSummary("Get Product By Id");

	}
}
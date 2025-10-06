using Catalog.API.Products.GetProductByCategory;

namespace Catalog.API.Products.UpdateProduct.DeleteProduct;

//public record DeleteProductRequest(Guid Id);

public class DeleteProductEndpoint : ICarterModule
{
	public void AddRoutes(IEndpointRouteBuilder app)
	{
		app.MapDelete("/products/{id}", async (Guid id, ISender sender, CancellationToken ct) =>
		{

			var command = new DeleteProductCommand(id);

			var result = await sender.Send(command, ct);

			return result.Match(_ => Results.NoContent(), ResponseHelper.Problem);

		}).WithName("DeleteProduct")
		.Produces<GetProductByCategoryResponse>(StatusCodes.Status204NoContent)
		.ProducesProblem(StatusCodes.Status400BadRequest)
		.ProducesProblem(StatusCodes.Status404NotFound)
		.WithDescription("Delete Product")
		.WithSummary("Delete Product");
	}
}
using BuildingBlocks.Common.Results;

namespace Catalog.API.Products.DeleteProduct;

public record DeleteProductCommand(Guid Id) : ICommand<Result<Success>>;
internal class DeleteProductCommandHandler(IDocumentSession session, ILogger<DeleteProductCommandHandler> logger)
: ICommandHandler<DeleteProductCommand, Result<Success>>
{
	public async ValueTask<Result<Success>> Handle(DeleteProductCommand command, CancellationToken ct)
	{
		var product = await session.LoadAsync<Product>(command.Id, ct);

		if (product is null)
		{
			logger.LogWarning("Product with id '{Id}' not found", command.Id);
			return AppErrors.ProductNotFound(command.Id);
		}

		session.Delete(product);

		await session.SaveChangesAsync(ct);

		return Result.Success;
	}
}
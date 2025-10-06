namespace Catalog.API.Products.UpdateProduct;

public record UpdateProductCommand(
    Guid Id,
    string Name,
    List<string> Category,
    string Description,
    string ImageFile,
    decimal Price) : ICommand<Result<Success>>;

//public record UpdateProductResult();

internal class UpdateProductCommandHandler(IDocumentSession session, ILogger<UpdateProductCommandHandler> logger)
: ICommandHandler<UpdateProductCommand, Result<Success>>
{
	public async ValueTask<Result<Success>> Handle(UpdateProductCommand command, CancellationToken ct)
	{
		logger.LogInformation(
				"{Class}.{Method} called with {@Command}",
				nameof(UpdateProductCommandHandler),
				nameof(Handle),
				command);

		var product = await session.LoadAsync<Product>(command.Id, ct);

		if (product is null)
		{
			logger.LogWarning("Product with id '{Id}' not found", command.Id);
			return AppErrors.ProductNotFound(command.Id);
		}

		product = command.Adapt<Product>();

		session.Update(product);

		await session.SaveChangesAsync(ct);

		return Result.Success;
	}
}
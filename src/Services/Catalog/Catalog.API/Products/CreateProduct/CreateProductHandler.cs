namespace Catalog.API.Products.CreateProduct;

public record CreateProductCommand(
    string Name,
    List<string> Category,
    string Description,
    string ImageFile,
    decimal Price) : ICommand<CreateProductResult>;

public record CreateProductResult(Guid Id);
internal class CreateProductCommandHandler(IDocumentSession session, ILogger<CreateProductCommandHandler> logger) : ICommandHandler<CreateProductCommand, CreateProductResult>
{
	public async ValueTask<CreateProductResult> Handle(CreateProductCommand command, CancellationToken ct)
	{
		logger.LogInformation(
				"{Class}.{Method} called with {@Command}",
				nameof(CreateProductCommandHandler),
				nameof(Handle),
				command);

		var product = command.Adapt<Product>();

		session.Store(product);

		await session.SaveChangesAsync(ct);

		return new(product.Id);
	}
}
namespace Catalog.API.Products.CreateProduct;

public record CreateProductCommand(
	string Name,
	List<string> Category,
	string Description,
	string ImageFile,
	decimal Price) : ICommand<CreateProductResult>;

public record CreateProductResult(Guid Id);

internal class CreateProductCommandHandler(IDocumentSession session) : ICommandHandler<CreateProductCommand, CreateProductResult>
{
    public async ValueTask<CreateProductResult> Handle(CreateProductCommand command, CancellationToken ct)
    {
        var product = command.Adapt<Product>();

        session.Store(product);

        await session.SaveChangesAsync(ct);

        return new(product.Id);
    }
}
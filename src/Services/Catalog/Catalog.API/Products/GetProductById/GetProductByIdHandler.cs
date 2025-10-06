namespace Catalog.API.Products.GetProductById;

public record GetProductByIdQuery(Guid Id) : IQuery<Result<GetProductByIdResult>>;
public record GetProductByIdResult(Product Product);
public class GetProductByIdQueryHandler(IDocumentSession session, ILogger<GetProductByIdQueryHandler> logger)
: IQueryHandler<GetProductByIdQuery, Result<GetProductByIdResult>>
{
	public async ValueTask<Result<GetProductByIdResult>> Handle(GetProductByIdQuery query, CancellationToken ct)
	{
		logger.LogInformation(
			"{Class}.{Method} called with {@Query}",
			nameof(GetProductByIdQueryHandler),
			nameof(Handle),
			query);

		var product = await session.LoadAsync<Product>(query.Id, ct);

		if (product is null)
		{
			logger.LogWarning("Product with id '{Id}' not found", query.Id);
			return AppErrors.ProductNotFound(query.Id);
		}

		return new GetProductByIdResult(product);
	}
}
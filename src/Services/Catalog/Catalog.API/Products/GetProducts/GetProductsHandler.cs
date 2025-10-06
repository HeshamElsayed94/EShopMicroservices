namespace Catalog.API.Products.GetProducts;

public record GetProductsQuery() : IQuery<GetProductsResult>;

public record GetProductsResult(IEnumerable<Product> Products);

internal class GetProductsQueryHandler(IDocumentSession session, ILogger<GetProductsQueryHandler> logger) : IQueryHandler<GetProductsQuery, GetProductsResult>
{
	public async ValueTask<GetProductsResult> Handle(GetProductsQuery query, CancellationToken ct)
	{
		logger.LogInformation(
			"{Class}.{Method} called with {@Query}",
			nameof(GetProductsQueryHandler),
			nameof(Handle),
			query);

		var products = await session.Query<Product>().ToListAsync(token: ct);

		return new(products);
	}
}
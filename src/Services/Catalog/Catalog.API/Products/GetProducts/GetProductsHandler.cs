namespace Catalog.API.Products.GetProducts;

public record GetProductsQuery() : IQuery<GetProductsResult>;

public record GetProductsResult(IEnumerable<Product> Products);

internal class GetProductsQueryHandler(IDocumentSession session) : IQueryHandler<GetProductsQuery, GetProductsResult>
{
	public async ValueTask<GetProductsResult> Handle(GetProductsQuery query, CancellationToken ct)
	{
		var products = await session.Query<Product>().ToListAsync(token: ct);

		return new(products);
	}
}
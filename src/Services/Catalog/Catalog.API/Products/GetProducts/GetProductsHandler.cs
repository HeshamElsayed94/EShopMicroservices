using Catalog.API.Common;

namespace Catalog.API.Products.GetProducts;

public record GetProductsQuery(int? PageNumber = 1, int? PageSize = 10) : IQuery<GetProductsResult>;

public record GetProductsResult(PagedResult<Product> Products);

internal class GetProductsQueryHandler(IDocumentSession session) : IQueryHandler<GetProductsQuery, GetProductsResult>
{
	public async ValueTask<GetProductsResult> Handle(GetProductsQuery query, CancellationToken ct)
	{
		var products = await session.Query<Product>().ToPagedListAsync(
			query.PageNumber!.Value,
			query.PageSize!.Value,
			ct);

		return new(new(products));
	}
}
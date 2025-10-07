namespace Catalog.API.Products.GetProductByCategory;

public record GetProductByCategoryQuery(string Category) : IQuery<GetProductByCategoryResult>;
public record GetProductByCategoryResult(IEnumerable<Product> Products);

internal class GetProductByCategoryQueryHandler(IDocumentSession session)
: IQueryHandler<GetProductByCategoryQuery, GetProductByCategoryResult>
{
	public async ValueTask<GetProductByCategoryResult> Handle(GetProductByCategoryQuery query, CancellationToken ct)
	{

		var products = await session.Query<Product>()
			.Where(x => x.Category.Contains(query.Category)).ToListAsync(token: ct);

		return new(products);
	}
}
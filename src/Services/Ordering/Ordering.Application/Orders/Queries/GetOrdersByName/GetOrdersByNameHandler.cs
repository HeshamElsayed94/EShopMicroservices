namespace Ordering.Application.Orders.Queries.GetOrdersByName;

internal class GetOrdersByNameQueryHandler(IApplicationDbContext context)
    : IQueryHandler<GetOrdersByNameQuery, GetOrdersByNameResult>
{
    public async ValueTask<GetOrdersByNameResult> Handle(GetOrdersByNameQuery query, CancellationToken ct)
    {
        var orders = await context.Orders.Where(x => x.OrderName.Value.Contains(query.Name))
            .Include(o => o.OrderItems)
            .AsNoTracking()
            .OrderBy(o => o.OrderName.Value)
            .ToListAsync(ct);

        return new(orders.ToOrderDtoList());
    }

}
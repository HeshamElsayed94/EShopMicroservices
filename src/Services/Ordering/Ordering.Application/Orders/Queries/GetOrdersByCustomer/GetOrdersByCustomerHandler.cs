namespace Ordering.Application.Orders.Queries.GetOrdersByCustomer;

internal class GetOrdersByCustomerQueryHandler(IApplicationDbContext context)
    : IQueryHandler<GetOrdersByCustomerQuery, GetOrdersByCustomerResult>
{
    public async ValueTask<GetOrdersByCustomerResult> Handle(GetOrdersByCustomerQuery query, CancellationToken ct)
    {
        var customerId = CustomerId.Of(query.CustomerId).Value;

        var orders = await context.Orders
            .Where(o => o.CustomerId.Equals(customerId))
            .Include(o => o.OrderItems)
            .AsNoTracking()
            .OrderBy(o => o.OrderName.Value)
            .ToListAsync(ct);

        return new(orders.ToOrderDtoList());
    }
}
using Ordering.Application.Pagination;

namespace Ordering.Application.Orders.Queries.GetOrders;

internal class GetOrdersQueryHandler(IApplicationDbContext context)
    : IQueryHandler<GetOrdersQuery, GetOrdersResult>
{
    public async ValueTask<GetOrdersResult> Handle(GetOrdersQuery query, CancellationToken ct)
    {
        var ordersQuery = context.Orders
            .Include(x => x.OrderItems)
            .AsNoTracking()
            .OrderBy(o => o.OrderName.Value);

        var orderPagedList = await PagedList<Order>.Create(ordersQuery, query.PaginationRequest.PageSize, query.PaginationRequest.PageNumber, ct);

        return new(PagedList<OrderDto>.Create(orderPagedList.Items.ToOrderDtoList(), orderPagedList.TotalCount, orderPagedList.PageSize, orderPagedList.PageNumber));

    }
}
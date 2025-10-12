using Ordering.Application.Pagination;

namespace Ordering.Application.Orders.Queries.GetOrders;

public record GetOrdersQuery(PaginationRequest PaginationRequest) : IQuery<GetOrdersResult>;

public record GetOrdersResult(PagedList<OrderDto> Orders);

public record PaginationRequest(int PageNumber = 1, int PageSize = 10);
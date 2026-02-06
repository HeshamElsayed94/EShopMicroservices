namespace Ordering.Application.Orders.Commands.CreateOrder;

public sealed record CreateOrderCommand(OrderDto Order) : ICommand<Result<CreateOrderResult>>;

public record CreateOrderResult(Guid Id);

namespace Ordering.Application.Orders.Commands.DeleteOrder;

internal class DeleteOrderCommandHandler(IApplicationDbContext context)
    : ICommandHandler<DeleteOrderCommand, Result<Success>>
{
    public async ValueTask<Result<Success>> Handle(DeleteOrderCommand command, CancellationToken ct)
    {
        var orderId = OrderId.Of(command.OrderId).Value;

        var order = await context.Orders.FindAsync([orderId], ct);

        if (order is null)
            return Error.NotFound(description: $"Order with id '{command.OrderId}' not found.");

        context.Orders.Remove(order);
        await context.SaveChangesAsync(ct);

        return Result.Success;
    }
}
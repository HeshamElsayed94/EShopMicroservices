namespace Ordering.Application.Orders.EventHandlers.Domain;

internal class OrderUpdatedEventHandler : INotificationHandler<OrderUpdatedEvent>
{
    public ValueTask Handle(OrderUpdatedEvent notification, CancellationToken ct)
    {
        return ValueTask.CompletedTask;
    }
}
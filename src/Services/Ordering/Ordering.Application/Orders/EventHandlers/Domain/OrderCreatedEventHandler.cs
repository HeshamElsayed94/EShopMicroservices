namespace Ordering.Application.Orders.EventHandlers.Domain;

internal class OrderCreatedEventHandler : INotificationHandler<OrderCreatedEvent>
{
    public ValueTask Handle(OrderCreatedEvent notification, CancellationToken ct)
    {
        return ValueTask.CompletedTask;
    }
}
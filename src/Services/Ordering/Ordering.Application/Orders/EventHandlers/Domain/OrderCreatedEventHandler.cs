using MassTransit;

namespace Ordering.Application.Orders.EventHandlers.Domain;

internal class OrderCreatedEventHandler(IPublishEndpoint publishEndpoint) : INotificationHandler<OrderCreatedEvent>
{
	public async ValueTask Handle(OrderCreatedEvent domainEvent, CancellationToken ct)
	{
		var orderCreatedIntegrationEvent = domainEvent.Order.ToOrderDto();
		await publishEndpoint.Publish(orderCreatedIntegrationEvent, ct);
	}
}
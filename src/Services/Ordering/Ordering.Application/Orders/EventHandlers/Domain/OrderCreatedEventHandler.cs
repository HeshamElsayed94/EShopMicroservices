using MassTransit;
using Microsoft.FeatureManagement;

namespace Ordering.Application.Orders.EventHandlers.Domain;

internal class OrderCreatedEventHandler(IPublishEndpoint publishEndpoint, IFeatureManager featureManager)
	: INotificationHandler<OrderCreatedEvent>
{
	public async ValueTask Handle(OrderCreatedEvent domainEvent, CancellationToken ct)
	{

		if (await featureManager.IsEnabledAsync("OrderFulfillment"))
		{
			var orderCreatedIntegrationEvent = domainEvent.Order.ToOrderDto();
			await publishEndpoint.Publish(orderCreatedIntegrationEvent, ct);
		}
	}
}
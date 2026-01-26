using BuildingBlocks.Messaging.Events;
using MassTransit;
using Microsoft.Extensions.Logging;
using Ordering.Application.Orders.Commands.CreateOrder;

namespace Ordering.Application.Orders.EventHandlers.Integration;

public class BasketCheckoutEventHandler(ISender sender, ILogger<BasketCheckoutEventHandler> logger)
: IConsumer<BasketCheckoutEvent>
{
	public async Task Consume(ConsumeContext<BasketCheckoutEvent> context)
	{
		var command = MapToCreateOrderCommand(context.Message);
		await sender.Send(command);
	}

	private static CreateOrderCommand MapToCreateOrderCommand(BasketCheckoutEvent message)
	{
		var addressDto = new AddressDto(
			message.FirstName,
			message.LastName,
			message.EmailAddress,
			message.AddressLine,
			message.Country,
			message.State,
			message.ZipCode);

		var paymentDto = new PaymentDto(
		message.CardName,
		message.CardNumber,
		message.Expiration,
		message.CVV,
		message.PaymentMethod);

		var orderId = Guid.NewGuid();

		var orderDto = new OrderDto(
			orderId,
			message.CustomerId,
			message.UserName,
			addressDto,
			addressDto,
			paymentDto,
			OrderStatus.Pending,
			[new(orderId,Guid.NewGuid(),2,500),
			new(orderId,Guid.NewGuid(),1,400)]);

		return new CreateOrderCommand(orderDto);
	}
}
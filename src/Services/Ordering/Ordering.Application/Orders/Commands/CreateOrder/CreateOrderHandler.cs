namespace Ordering.Application.Orders.Commands.CreateOrder;

internal class CreateOrderCommandHandler(IApplicationDbContext context)
	: ICommandHandler<CreateOrderCommand, Result<CreateOrderResult>>
{
	public async ValueTask<Result<CreateOrderResult>> Handle(CreateOrderCommand command, CancellationToken ct)
	{
		var orderResult = CreateNewOrderResult(command.Order);

		if (!orderResult.ISuccess)
			return orderResult.Errors!.ToList();

		await context.Orders.AddAsync(orderResult.Value, ct);

		await context.SaveChangesAsync(ct);

		return new CreateOrderResult(orderResult.Value.Id.Value);
	}

	private static Result<Order> CreateNewOrderResult(OrderDto order)
	{
		var shippingAddressResult = CreateShippingAddressResult(order.ShippingAddress);
		var billingAddressResult = CreateBillingAddressResult(order.BillingAddress);
		var paymentResult = CreatePaymentResult(order.Payment);
		var orderNameResult = OrderName.Of(order.OrderName);
		var combinedResult = Result.Combine(shippingAddressResult, billingAddressResult, paymentResult, orderNameResult);

		if (!combinedResult.ISuccess)
			return combinedResult.Errors!.ToList();

		var newOrderResult = Order.Create(OrderId.Of(Guid.NewGuid()).Value,
			CustomerId.Of(order.CustomerId).Value,
			orderNameResult.Value,
			shippingAddressResult.Value,
			billingAddressResult.Value,
			paymentResult.Value
			);

		foreach (var orderItemDto in order.OrderItems)
		{
			var productId = ProductId.Of(orderItemDto.ProductId);

			if (!productId.ISuccess)
				return productId.Errors!.ToList();

			newOrderResult.Value.Add(productId.Value, orderItemDto.Quantity, orderItemDto.Price);
		}

		return newOrderResult;
	}

	private static Result<Payment> CreatePaymentResult(PaymentDto payment) =>
			Payment.Of(payment.CardName, payment.CardNumber, payment.Expiration, payment.Cvv, payment.PaymentMethod);

	private static Result<Address> CreateShippingAddressResult(AddressDto shippingAddress) => Address.Of(shippingAddress.FirstName, shippingAddress.LastName, shippingAddress.EmaiAddress, shippingAddress.AddressLine, shippingAddress.Country, shippingAddress.State, shippingAddress.ZipCode);

	private static Result<Address> CreateBillingAddressResult(AddressDto billinAddress) => Address.Of(billinAddress.FirstName, billinAddress.LastName, billinAddress.EmaiAddress, billinAddress.AddressLine, billinAddress.Country, billinAddress.State, billinAddress.ZipCode);
}
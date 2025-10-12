namespace Ordering.Application.Orders.Commands.UpdateOrder;

public class UpdateOrderCommandHandler(IApplicationDbContext context)
    : ICommandHandler<UpdateOrderCommand, Result<Success>>
{
    public async ValueTask<Result<Success>> Handle(UpdateOrderCommand command, CancellationToken ct)
    {
        var orderId = OrderId.Of(command.Order.Id).Value;

        var order = await context.Orders.FindAsync([orderId], ct);

        if (order is null)
            return Error.NotFound(description: $"Order with id '{command.Order.Id}' not found.");

        var updateResult = UpdateOrderWithNewValuesResult(order, command.Order);

        if (!updateResult.ISuccess)
            return updateResult.Errors!.ToList();

        context.Orders.Update(order);
        await context.SaveChangesAsync(ct);

        return Result.Success;

    }

    private Result<Success> UpdateOrderWithNewValuesResult(Order order, OrderDto orderDto)
    {
        var shippingAddressResult = CreateShippingAddressResult(orderDto.ShippingAddress);
        var billingAddressResult = CreateBillingAddressResult(orderDto.BillingAddress);
        var paymentResult = CreatePaymentResult(orderDto.Payment);

        var orderNameResult = OrderName.Of(orderDto.OrderName);

        var combinedResult = Result.Combine(shippingAddressResult, billingAddressResult, paymentResult, orderNameResult);

        if (!combinedResult.ISuccess)
            return combinedResult.Errors!.ToList();

        return order.Update(orderNameResult.Value, shippingAddressResult.Value, billingAddressResult.Value, paymentResult.Value, orderDto.Status);
    }

    private static Result<Payment> CreatePaymentResult(PaymentDto payment) =>
           Payment.Of(payment.CardName, payment.CardNumber, payment.Expiration, payment.Cvv, payment.PaymentMethod);

    private static Result<Address> CreateShippingAddressResult(AddressDto shippingAddress) => Address.Of(shippingAddress.FirstName, shippingAddress.LastName, shippingAddress.EmaiAddress, shippingAddress.AddressLine, shippingAddress.Country, shippingAddress.State, shippingAddress.ZipCode);

    private static Result<Address> CreateBillingAddressResult(AddressDto billinAddress) => Address.Of(billinAddress.FirstName, billinAddress.LastName, billinAddress.EmaiAddress, billinAddress.AddressLine, billinAddress.Country, billinAddress.State, billinAddress.ZipCode);
}
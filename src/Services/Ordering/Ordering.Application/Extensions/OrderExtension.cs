namespace Ordering.Application.Extensions;

public static class OrderExtension
{
    public static IEnumerable<OrderDto> ToOrderDtoList(this IEnumerable<Order> orders)
    {
        return orders.Select(o => new OrderDto(
            o.Id.Value,
        o.CustomerId.Value,
        o.OrderName.Value,
            new(o.ShippingAddress.FirstName, o.ShippingAddress.LastName, o.ShippingAddress.EmailAddress, o.ShippingAddress.AddressLine, o.ShippingAddress.Country, o.ShippingAddress.State, o.ShippingAddress.ZipCode),
            new(o.BillingAddress.FirstName, o.BillingAddress.LastName, o.BillingAddress.EmailAddress, o.BillingAddress.AddressLine, o.BillingAddress.Country, o.BillingAddress.State, o.BillingAddress.ZipCode),
            new(o.Payment.CardName, o.Payment.CardNumber, o.Payment.Expiration, o.Payment.CVV, o.Payment.PaymentMethod),
            o.Status,
            [.. o.OrderItems.Select(oi => new OrderItemDto(oi.OrderId.Value, oi.ProductId.Value, oi.Quantity, oi.Price))]

            ));
    }
}
namespace Ordering.Application.Dtos;

public sealed record OrderDto(
	Guid Id,
	Guid CustomerId,
	string OrderName,
	AddressDto ShippingAddress,
	AddressDto BillingAddress,
	PaymentDto Payment,
	OrderStatus Status,
	List<OrderItemDto> OrderItems
	);
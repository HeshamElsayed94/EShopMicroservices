namespace Ordering.Application.Dtos;

public sealed record OrderItemDto(Guid OrderId, Guid ProductId, int Quantity, decimal Price);

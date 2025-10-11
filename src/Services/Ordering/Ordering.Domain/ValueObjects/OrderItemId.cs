namespace Ordering.Domain.ValueObjects;

public record OrderItemId
{
    [JsonConstructor]
    private OrderItemId(Guid value) => Value = value;

    public Guid Value { get; init; }

    public static Result<OrderItemId> Of(Guid value)
    {

        if (value == Guid.Empty)
            return Error.Validation(description: "OrderItemId cannot be empty.");

        return new OrderItemId(value);

    }
}
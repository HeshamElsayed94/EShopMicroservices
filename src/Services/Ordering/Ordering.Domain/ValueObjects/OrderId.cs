namespace Ordering.Domain.ValueObjects;

public record OrderId
{
    [JsonConstructor]
    private OrderId(Guid value) => Value = value;

    public Guid Value { get; init; }

    public static Result<OrderId> Of(Guid value)
    {
        if (value == Guid.Empty)
            return Error.Validation(description: "OrderId cannot be empty.");

        return new OrderId(value);

    }
}
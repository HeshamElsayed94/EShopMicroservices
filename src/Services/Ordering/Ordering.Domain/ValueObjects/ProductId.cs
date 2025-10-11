namespace Ordering.Domain.ValueObjects;

public record ProductId
{
    [JsonConstructor]
    private ProductId(Guid value) => Value = value;

    public Guid Value { get; init; }

    public static Result<ProductId> Of(Guid value)
    {
        if (value == Guid.Empty)
            return Error.Validation(description: "ProductId cannot be empty.");

        return new ProductId(value);

    }
}
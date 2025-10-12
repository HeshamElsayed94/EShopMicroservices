namespace Ordering.Domain.Models;

public sealed class Product : Entity<ProductId>
{
    public string Name { get; private set; } = null!;

    public decimal Price { get; private set; }

    public static Result<Product> Create(ProductId id, string name, decimal price)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name, nameof(name));
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(price, nameof(price));

        return new Product()
        {
            Id = id,
            Name = name,
            Price = price
        };
    }

    private Product()
    { }
}
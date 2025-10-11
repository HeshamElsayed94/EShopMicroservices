namespace Ordering.Domain.Models;

public sealed class Product : Entity<ProductId>
{
    public string Name { get; private set; } = null!;

    public decimal Price { get; private set; }

    public static Result<Product> Create(string name, decimal price)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name, nameof(name));
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(price, nameof(price));

        return new Product() { Name = name, Price = price };
    }
}
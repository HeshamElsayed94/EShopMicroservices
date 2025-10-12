namespace Ordering.Domain.Models;

public sealed class Customer : Entity<CustomerId>
{
    public string Name { get; private set; } = null!;

    public string Email { get; private set; } = null!;

    //[JsonConstructor]
    private Customer()
    {

    }

    public static Customer Create(CustomerId id, string name, string email)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name, nameof(name));
        ArgumentException.ThrowIfNullOrWhiteSpace(email, nameof(email));

        return new()
        {
            Id = id,
            Name = name,
            Email = email
        };
    }
}
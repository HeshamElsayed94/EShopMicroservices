namespace Basket.API.Errors;

public static class BasketErrors
{
    public static Error BasketNotFound(string userName) => Error.NotFound(description: $"Basket for userName {userName} not found.");
}
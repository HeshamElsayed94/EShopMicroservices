namespace Basket.API.Models;

public class ShoppingCartItem
{
    public Guid ProductId { get; set; }

    public string ProductName { get; set; } = null!;

    public string Color { get; set; } = null!;

    public int Quantity { get; set; }

    public decimal Price { get; set; }

}
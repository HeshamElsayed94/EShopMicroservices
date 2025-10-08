namespace Basket.API.Models;

public class ShoppingCart
{
    public string UserName { get; set; } = null!;

    public ShoppingCart(string userName) => UserName = userName;

    public List<ShoppingCartItem> Items { get; set; } = [];

    public decimal TotalPrice => Items.Sum(x => x.Price * x.Quantity);

    // for mapping
    public ShoppingCart()
    {

    }
}
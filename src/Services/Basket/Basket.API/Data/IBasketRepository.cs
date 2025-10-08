namespace Basket.API.Data;

public interface IBasketRepository
{
    Task<ShoppingCart?> GetBasket(string userName, CancellationToken ct = default);

    Task<ShoppingCart> StoreBasket(ShoppingCart basket, CancellationToken ct = default);

    Task<bool> IsBasketExist(string userName, CancellationToken ct = default);

    Task DeleteBasket(string UserName, CancellationToken ct = default);
}
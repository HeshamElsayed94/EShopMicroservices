namespace Basket.API.Data;

public class BasketRepository(IDocumentSession session) : IBasketRepository
{
    public async Task DeleteBasket(string UserName, CancellationToken ct = default)
    {
        session.Delete<ShoppingCart>(UserName);
        await session.SaveChangesAsync(ct);
    }

    public async Task<ShoppingCart?> GetBasket(string userName, CancellationToken ct = default)
    {
        var basket = await session.LoadAsync<ShoppingCart>(userName, ct);

        return basket;
    }

    public async Task<bool> IsBasketExist(string userName, CancellationToken ct = default)
    {
        return await session.Query<ShoppingCart>().AnyAsync(x => x.UserName.Equals(userName), token: ct);
    }

    public async Task<ShoppingCart> StoreBasket(ShoppingCart basket, CancellationToken ct = default)
    {
        session.Store(basket);
        await session.SaveChangesAsync(ct);
        return basket;
    }
}
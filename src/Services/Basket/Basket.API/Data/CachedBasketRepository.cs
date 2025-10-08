namespace Basket.API.Data;

public class CachedBasketRepository(IBasketRepository repository, IDistributedCache cache) : IBasketRepository
{

    public async Task<bool> IsBasketExist(string userName, CancellationToken ct = default)
    {
        var isExist = await cache.GetAsync(userName, ct) is not null;

        if (isExist)
            return true;

        return await repository.IsBasketExist(userName, ct);
    }

    public async Task<ShoppingCart?> GetBasket(string userName, CancellationToken ct = default)
    {
        var cachedBasket = await cache.GetStringAsync(userName, ct);

        if (!string.IsNullOrWhiteSpace(cachedBasket))
            return JsonSerializer.Deserialize<ShoppingCart>(cachedBasket);

        var basket = await repository.GetBasket(userName, ct);
        await cache.SetStringAsync(userName, JsonSerializer.Serialize(basket), ct);
        return basket;
    }

    public async Task<ShoppingCart> StoreBasket(ShoppingCart basket, CancellationToken ct = default)
    {
        await repository.StoreBasket(basket, ct);
        await cache.SetStringAsync(basket.UserName, JsonSerializer.Serialize(basket), ct);
        return basket;
    }

    public async Task DeleteBasket(string UserName, CancellationToken ct = default)
    {
        await repository.DeleteBasket(UserName, ct);
        await cache.RemoveAsync(UserName, ct);
    }
}
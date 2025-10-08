namespace Basket.API.Basket.GetBasket;

public record GetBasketQuery(string UserName) : IQuery<Result<GetBasketResult>>;

public record GetBasketResult(ShoppingCart Cart);

internal class GetBasketQueryHandler(IBasketRepository repository, ILogger<GetBasketQueryHandler> logger)
    : IQueryHandler<GetBasketQuery, Result<GetBasketResult>>
{
    public async ValueTask<Result<GetBasketResult>> Handle(GetBasketQuery query, CancellationToken ct)
    {
        var basket = await repository.GetBasket(query.UserName, ct);

        if (basket is null)
        {
            logger.LogWarning("Basket for userName '{UserName} not found'", query.UserName);
            return BasketErrors.BasketNotFound(query.UserName);
        }

        return new GetBasketResult(basket);
    }
}
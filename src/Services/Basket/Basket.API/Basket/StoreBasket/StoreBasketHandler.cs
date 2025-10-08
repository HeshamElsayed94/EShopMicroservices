namespace Basket.API.Basket.StoreBasket;

public record StoreBasketCommand(ShoppingCart Cart) : ICommand<StoreBasketResult>;

public record StoreBasketResult(string UserName);

internal class StoreBasketCommandHandler(IBasketRepository repository) : ICommandHandler<StoreBasketCommand, StoreBasketResult>
{
    public async ValueTask<StoreBasketResult> Handle(StoreBasketCommand command, CancellationToken ct)
    {
        var cart = await repository.StoreBasket(command.Cart, ct);

        return new StoreBasketResult(cart.UserName);
    }
}
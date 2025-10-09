using Discount.Grpc;

namespace Basket.API.Basket.StoreBasket;

public record StoreBasketCommand(ShoppingCart Cart) : ICommand<StoreBasketResult>;

public record StoreBasketResult(string UserName);

internal class StoreBasketCommandHandler(IBasketRepository repository,
    DiscountProtoService.DiscountProtoServiceClient discountProto)
    : ICommandHandler<StoreBasketCommand, StoreBasketResult>
{
    public async ValueTask<StoreBasketResult> Handle(StoreBasketCommand command, CancellationToken ct)
    {
        await DeductDiscount(command.Cart, ct);

        var cart = await repository.StoreBasket(command.Cart, ct);

        return new StoreBasketResult(cart.UserName);
    }

    private async Task DeductDiscount(ShoppingCart cart, CancellationToken ct)
        => cart.Items.ForEach(async x =>
            {
                var coupon = await discountProto
                    .GetDiscountAsync(new() { ProductName = x.ProductName },
                    cancellationToken: ct);

                x.Price -= coupon.Amount;
            });
}
namespace Basket.API.Basket.CheckoutBasket;

public record CheckoutBasketCommand(BasketCheckoutDto BasketCheckoutDto) : ICommand<Result<Success>>;

public class CheckoutBasketHandler(IBasketRepository basketRepository, IPublishEndpoint publishEndpoint)
: ICommandHandler<CheckoutBasketCommand, Result<Success>>
{
	public async ValueTask<Result<Success>> Handle(CheckoutBasketCommand command, CancellationToken ct)
	{
		var basketExist = await basketRepository.IsBasketExist(command.BasketCheckoutDto.UserName, ct);

		if (!basketExist)
			return Error.NotFound("Basket not found");

		var eventMessage = command.BasketCheckoutDto.Adapt<BasketCheckoutEvent>();

		await publishEndpoint.Publish(eventMessage, ct);

		await basketRepository.DeleteBasket(command.BasketCheckoutDto.UserName, ct);

		return Result.Success;
	}
}
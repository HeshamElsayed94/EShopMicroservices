namespace Basket.API.Basket.DeleteBasket;

public record DeleteBasketCommand(string UserName) : ICommand<Result<Success>>;

//public record DeleteBasketResult();

internal class DeleteBasketCommandHandler(IBasketRepository repository, ILogger<DeleteBasketCommandHandler> logger)
    : ICommandHandler<DeleteBasketCommand, Result<Success>>
{
    public async ValueTask<Result<Success>> Handle(DeleteBasketCommand command, CancellationToken ct)
    {

        if (!await repository.IsBasketExist(command.UserName, ct))
        {
            logger.LogWarning("Basket for userName '{UserName} not found'", command.UserName);
            return BasketErrors.BasketNotFound(command.UserName);
        }

        await repository.DeleteBasket(command.UserName, ct);

        return Result.Success;
    }
}
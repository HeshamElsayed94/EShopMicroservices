using FluentValidation;

namespace Ordering.Application.Orders.Commands.CreateOrder;

public sealed record CreateOrderCommand(OrderDto Order) : ICommand<Result<CreateOrderResult>>;

public record CreateOrderResult(Guid Id);

public class CreateOrderRequestValidator : AbstractValidator<CreateOrderCommand>
{
    public CreateOrderRequestValidator()
    {
        RuleFor(x => x.Order.OrderName).NotEmpty();
        RuleFor(x => x.Order.CustomerId).NotEmpty();
        RuleFor(x => x.Order.OrderItems).NotEmpty();

        When(x => x.Order.OrderItems is not null, () =>
        {
            RuleFor(i => i.Order.OrderItems.Count).GreaterThanOrEqualTo(1);
        });

    }
}
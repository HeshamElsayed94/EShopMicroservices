using FluentValidation;

namespace Ordering.Application.Orders.Commands.UpdateOrder;

public record UpdateOrderCommand(OrderDto Order) : ICommand<Result<Success>>;
public class UpdateOrderRequestValidator : AbstractValidator<UpdateOrderCommand>
{
    public UpdateOrderRequestValidator()
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
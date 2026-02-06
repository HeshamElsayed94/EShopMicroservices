using Ordering.Application.Orders.Commands.UpdateOrder;

namespace Ordering.API.Endpoints;

public record UpdateOrderRequest(OrderDto Order);

public class UpdateOrderRequestValidator : AbstractValidator<UpdateOrderRequest>
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

//public record UpdateOrderResponse();

public class UpdateOrder : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPut("orders", async (UpdateOrderRequest request, ISender sender, CancellationToken ct) =>
        {
            var command = request.Adapt<UpdateOrderCommand>();

            var result = await sender.Send(command, ct);

            return result.Match(_ => Results.NoContent(), ResponseHelper.Problem);
        }).WithName("UpdateOrder")
        .Produces(StatusCodes.Status204NoContent)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status401Unauthorized)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .WithSummary("Update Order")
        .WithDescription("Update Order");
    }
}
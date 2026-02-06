namespace Ordering.API.Endpoints;

public record CreateOrderRequest(OrderDto Order);

public class CreateOrderRequestValidator : AbstractValidator<CreateOrderRequest>
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

public record CreateOrderResponse(Guid Id);
public class CreateOrder : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("orders", async (CreateOrderRequest request, ISender sender, CancellationToken ct) =>
        {
            var command = request.Adapt<CreateOrderCommand>();

            var result = await sender.Send(command, ct);

            return result.Match(val =>
            {
                var response = val.Adapt<CreateOrderResponse>();
                return Results.Created($"orders/{response.Id}", response);
            },
            ResponseHelper.Problem);

        }).WithName("CreteOrder")
        .Produces<CreateOrderResponse>(StatusCodes.Status201Created)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status422UnprocessableEntity)
        .WithSummary("Create Order")
        .WithDescription("Create Order");
    }
}
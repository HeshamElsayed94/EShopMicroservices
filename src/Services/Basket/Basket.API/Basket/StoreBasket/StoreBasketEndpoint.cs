namespace Basket.API.Basket.StoreBasket;

public record StoreBasketRequest(ShoppingCart Cart);

public class StoreBasketRequestValidator : AbstractValidator<StoreBasketRequest>
{
    public StoreBasketRequestValidator()
    {
        RuleFor(x => x.Cart).NotNull();
        RuleFor(x => x.Cart.UserName).NotEmpty();
        RuleFor(x => x.Cart.Items.Count).GreaterThanOrEqualTo(1);
    }
}

public record StoreBasketResponse(string UserName);
public class StoreBasketEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/basket", async (StoreBasketRequest request, ISender sender, CancellationToken ct) =>
        {
            var command = request.Adapt<StoreBasketCommand>();

            var result = await sender.Send(command, ct);

            var response = result.Adapt<StoreBasketResponse>();

            return Results.Created($"basket/{response.UserName}", response);
        })
         .WithName("StoreBasket")
         .Produces<StoreBasketResponse>(StatusCodes.Status201Created)
         .ProducesProblem(StatusCodes.Status400BadRequest)
         .ProducesProblem(StatusCodes.Status422UnprocessableEntity)
         .ProducesProblem(StatusCodes.Status404NotFound)
         .WithDescription("Store Basket")
         .WithSummary("Store Basket");
    }

}
namespace Basket.API.Basket.DeleteBasket;

//public record DeleteBasketRequest(string UserName);

//public record DeleteBasketResponse();

public class DeleteBasketEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete("/basket/{userName}", async (string userName, ISender sender, CancellationToken ct) =>
        {
            var command = new DeleteBasketCommand(userName);

            var result = await sender.Send(command, ct);

            return result.Match(_ => Results.NoContent(), ResponseHelper.Problem);
        })
         .WithName("DeleteBasket")
         .Produces(StatusCodes.Status204NoContent)
         .ProducesProblem(StatusCodes.Status400BadRequest)
         .ProducesProblem(StatusCodes.Status404NotFound)
         .WithDescription("Delete Basket")
         .WithSummary("Delete Basket");
    }
}
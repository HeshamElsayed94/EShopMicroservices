using BuildingBlocks.Helper;

namespace Basket.API.Basket.GetBasket;

//public record GetBasketRequest(string UserName);
public record GetBasketResponse(ShoppingCart Cart);

public class GetBasketEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/basket/{userName}", async (string userName, ISender sender, CancellationToken ct) =>
        {
            var query = new GetBasketQuery(userName);

            var result = await sender.Send(query, ct);

            return result.Match(
                val => Results.Ok(val.Adapt<GetBasketResponse>()),
                ResponseHelper.Problem);

        })
         .WithName("GetBasketByUserName")
         .Produces<GetBasketResponse>(StatusCodes.Status200OK)
         .ProducesProblem(StatusCodes.Status400BadRequest)
         .ProducesProblem(StatusCodes.Status422UnprocessableEntity)
         .ProducesProblem(StatusCodes.Status404NotFound)
         .WithDescription("Get Basket By UserName")
         .WithSummary("Get Basket By UserName");
    }
}
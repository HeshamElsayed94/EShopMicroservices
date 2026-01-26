using Microsoft.AspNetCore.Mvc;

namespace Basket.API.Basket.CheckoutBasket;

public record CheckoutBasketRequest(BasketCheckoutDto BasketCheckoutDto);
//public record CheckoutBasketResponse(bool ISuccess);

public class CheckoutBasketRequestValidator : AbstractValidator<CheckoutBasketRequest>
{
	public CheckoutBasketRequestValidator()
	{
		RuleFor(x => x.BasketCheckoutDto).NotNull().WithMessage("BasketCheckoutDto can't be null");
		RuleFor(x => x.BasketCheckoutDto.UserName).NotEmpty();
	}
}

public class CheckoutBasketEndpoint : ICarterModule
{
	public void AddRoutes(IEndpointRouteBuilder app)
	{
		app.MapPost("/basket/checkout", async ([FromBody] BasketCheckoutDto request, ISender sender, CancellationToken ct) =>
		{
			var command = request.Adapt<CheckoutBasketCommand>();
			var result = await sender.Send(command, ct);

			return result.Match(_ => Results.Created(), ResponseHelper.Problem);
		}
		).WithName("CheckoutBasket")
		.Produces(StatusCodes.Status201Created)
		.ProducesProblem(StatusCodes.Status404NotFound)
		.ProducesProblem(StatusCodes.Status400BadRequest)
		.ProducesValidationProblem(StatusCodes.Status422UnprocessableEntity)
		.WithSummary("Checkout Basket")
		.WithDescription("Checkout Basket");
	}
}
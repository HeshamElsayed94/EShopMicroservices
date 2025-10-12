namespace Ordering.Application.Dtos;

public sealed record PaymentDto(
	string CardName,
	string CardNumber,
	string Expiration,
	string Cvv,
	int PaymentMethod);
namespace Ordering.Domain.ValueObjects;

public record Payment
{
	[JsonConstructor]
	private Payment(
		string cardName,
		string cardNumber,
		string expiration,
		string cVV,
		int paymentMethod)
	{
		CardName = cardName;
		CardNumber = cardNumber;
		Expiration = expiration;
		CVV = cVV;
		PaymentMethod = paymentMethod;
	}

	protected Payment() { }

	public static Result<Payment> Of(
		string cardName,
		string cardNumber,
		string expiration,
		string cVV,
		int paymentMethod)
	{
		ArgumentException.ThrowIfNullOrWhiteSpace(cardName, nameof(cardName));
		ArgumentException.ThrowIfNullOrWhiteSpace(cardNumber, nameof(cardNumber));
		ArgumentException.ThrowIfNullOrWhiteSpace(cVV, nameof(cVV));
		ArgumentException.ThrowIfNullOrWhiteSpace(expiration, nameof(expiration));
		ArgumentOutOfRangeException.ThrowIfNotEqual(cVV.Length, 3);

		return new Payment(cardName, cardNumber, expiration, cVV, paymentMethod);
	}

	public string CardName { get; init; }
	public string CardNumber { get; init; }
	public string Expiration { get; init; }
	public string CVV { get; init; }
	public int PaymentMethod { get; init; }
}
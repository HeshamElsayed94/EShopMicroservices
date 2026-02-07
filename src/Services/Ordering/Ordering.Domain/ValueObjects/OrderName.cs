namespace Ordering.Domain.ValueObjects;

public record OrderName
{
	private const int DefaultLength = 3;

	[JsonConstructor]
	private OrderName(string value) => Value = value;

	public string Value { get; init; }

	public static Result<OrderName> Of(string value)
	{
		ArgumentException.ThrowIfNullOrWhiteSpace(value);

		ArgumentOutOfRangeException.ThrowIfLessThan(value.Length, DefaultLength);

		return new OrderName(value);

	}
}
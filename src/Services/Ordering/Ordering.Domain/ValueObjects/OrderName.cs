namespace Ordering.Domain.ValueObjects;

public record OrderName
{
	private const int DefaultLength = 5;
	[JsonConstructor]
	private OrderName(string value) => Value = value;

	public string Value { get; init; }

	public static Result<OrderName> Of(string value)
	{
		ArgumentException.ThrowIfNullOrWhiteSpace(value, nameof(value));

		ArgumentOutOfRangeException.ThrowIfLessThan(value.Length, DefaultLength);

		return new OrderName(value);

	}
}
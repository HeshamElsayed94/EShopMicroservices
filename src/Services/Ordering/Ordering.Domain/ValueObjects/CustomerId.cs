namespace Ordering.Domain.ValueObjects;

public record CustomerId
{
	[JsonConstructor]
	private CustomerId(Guid value) => Value = value;

	public Guid Value { get; init; }

	public static Result<CustomerId> Of(Guid value)
	{
		ArgumentNullException.ThrowIfNull(value, nameof(value));

		if (value == Guid.Empty)
			return Error.Validation(description: "CustomerId cannot be empty.");

		return new CustomerId(value);
	}
}
namespace Ordering.Domain.ValueObjects;

public record Address
{
	[JsonConstructor]
	private Address(
		string firstName,
		string lastName,
		string emailAddress,
		string addressLine,
		string country,
		string state,
		string zipCode)
	{
		FirstName = firstName;
		LastName = lastName;
		EmailAddress = emailAddress;
		AddressLine = addressLine;
		Country = country;
		State = state;
		ZipCode = zipCode;
	}

	protected Address()
	{

	}

	public static Result<Address> Of(string firstName,
		string lastName,
		string emailAddress,
		string addressLine,
		string country,
		string state,
		string zipCode)
	{
		ArgumentException.ThrowIfNullOrWhiteSpace(emailAddress, nameof(emailAddress));
		ArgumentException.ThrowIfNullOrWhiteSpace(addressLine, nameof(addressLine));

		return new Address(firstName, lastName, emailAddress, addressLine, country, state, zipCode);
	}

	public string FirstName { get; init; }
	public string LastName { get; init; }
	public string EmailAddress { get; init; }
	public string AddressLine { get; init; }
	public string Country { get; init; }
	public string State { get; init; }
	public string ZipCode { get; init; }

}
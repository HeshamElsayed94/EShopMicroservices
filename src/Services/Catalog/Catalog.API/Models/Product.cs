namespace Catalog.API.Models;

public class Product
{
	public Guid Id { get; set; }

	public string Name { get; set; } = null!;

	public List<string> Category { get; set; } = [];

	public string Description { get; set; } = null!;

	public string ImageFile { get; set; } = null!;

	public decimal Price { get; set; }
}
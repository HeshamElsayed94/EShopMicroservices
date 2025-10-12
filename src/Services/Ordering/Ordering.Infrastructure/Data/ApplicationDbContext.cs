namespace Ordering.Infrastructure.Data;

internal class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<Customer> Customers { get; private set; }

    public DbSet<Product> Products { get; private set; }

    public DbSet<Order> Orders { get; private set; }

    public DbSet<OrderItem> OrderItems { get; private set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(builder);
    }
}
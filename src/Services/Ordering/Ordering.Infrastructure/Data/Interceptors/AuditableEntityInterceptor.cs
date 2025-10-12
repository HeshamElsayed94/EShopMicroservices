namespace Ordering.Infrastructure.Data.Interceptors;

internal class AuditableEntityInterceptor : SaveChangesInterceptor
{

    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        UpdateEntities(eventData.Context);

        return base.SavingChanges(eventData, result);
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
    {
        UpdateEntities(eventData.Context);
        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    private void UpdateEntities(DbContext? context)
    {
        if (context is null)
            return;

        foreach (var entry in context.ChangeTracker.Entries<IEntity>())
        {
            if (entry.State is EntityState.Added)
            {
                entry.Entity.CreatedBy = "hesham";
                entry.Entity.CreatedAt = DateTime.UtcNow;
            }

            if (entry.State is EntityState.Modified or EntityState.Added || entry.HasChangedOwnedEntities())
            {
                entry.Entity.LastModifiedBy = "hesham";
                entry.Entity.LastModified = DateTime.UtcNow;
            }

        }

    }
}

public static class Extensions
{
    public static bool HasChangedOwnedEntities(this EntityEntry entry)
    {
        return entry.References.Any(r =>
        r.TargetEntry is not null &&
        r.TargetEntry.Metadata.IsOwned() &&
        r.TargetEntry.State is EntityState.Added or EntityState.Modified or EntityState.Deleted);
    }

}
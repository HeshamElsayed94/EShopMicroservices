using Mediator;

namespace Ordering.Domain.Abstraction;

public interface IDomainEvent : INotification
{
    Guid EventId => Guid.NewGuid();

    DateTime OccurredOn => DateTime.UtcNow;

    string EventType => GetType().AssemblyQualifiedName!;
}
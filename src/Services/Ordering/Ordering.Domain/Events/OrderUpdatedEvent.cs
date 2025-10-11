using Ordering.Domain.Models;

namespace Ordering.Domain.Events;

public sealed record OrderUpdatedEvent(Order Order) : IDomainEvent;
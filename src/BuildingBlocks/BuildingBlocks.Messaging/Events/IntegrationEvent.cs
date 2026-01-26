namespace BuildingBlocks.Messaging.Events;

public record IntegrationEvent
{
	public Guid Id => Guid.NewGuid();
	public DateTime OccuredOn => DateTime.UtcNow;
	public string EventType => GetType().AssemblyQualifiedName;
}
namespace VendorHub.Domain.Common;

public abstract class AggregateRoot : BaseEntity
{
    public Guid Version { get; protected set; } = Guid.NewGuid();
    private readonly List<IDomainEvent> _domainEvents = new();

    public IReadOnlyList<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();
    public void AddDomainEvent(IDomainEvent domainEvent) => _domainEvents.Add(domainEvent);
    public void ClearDomainEvents() => _domainEvents.Clear();
}

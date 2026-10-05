namespace VendorHub.Domain.Events;

using VendorHub.Domain.Common;

public record ProductCreatedDomainEvent(Guid ProductId, string Name, Guid VendorId) : DomainEvent;

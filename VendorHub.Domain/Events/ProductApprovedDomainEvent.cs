namespace VendorHub.Domain.Events;

using VendorHub.Domain.Common;

public record ProductApprovedDomainEvent(Guid ProductId, Guid VendorId) : DomainEvent;

namespace VendorHub.Domain.Events;

using VendorHub.Domain.Common;

public record ProductRejectedDomainEvent(Guid ProductId, Guid VendorId, string Reason) : DomainEvent;

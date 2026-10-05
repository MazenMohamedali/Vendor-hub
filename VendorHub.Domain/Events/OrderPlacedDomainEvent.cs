namespace VendorHub.Domain.Events;

using VendorHub.Domain.Common;

public record OrderPlacedDomainEvent(Guid OrderId, Guid CustomerId, decimal TotalAmount) : DomainEvent;

namespace VendorHub.Domain.Events;

using VendorHub.Domain.Common;

public record ProductStockDepletedDomainEvent(Guid ProductId, string ProductName, Guid VendorId) : DomainEvent;

namespace VendorHub.Domain.Events;

using VendorHub.Domain.Common;
using VendorHub.Domain.Enums;

public record OrderStatusChangedDomainEvent(Guid OrderId, OrderStatus NewStatus) : DomainEvent;


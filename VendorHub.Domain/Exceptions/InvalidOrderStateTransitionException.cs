namespace VendorHub.Domain.Exceptions;

using VendorHub.Domain.Enums;

public class InvalidOrderStateTransitionException : DomainException
{
    public Guid OrderId { get; }
    public OrderStatus CurrentStatus { get; }
    public OrderStatus AttemptedStatus { get; }

    public InvalidOrderStateTransitionException(Guid orderId, OrderStatus currentStatus, OrderStatus attemptedStatus)
        : base($"Cannot transition order '{orderId}' from status '{currentStatus}' to '{attemptedStatus}'.", "INVALID_ORDER_TRANSITION")
    {
        OrderId = orderId;
        CurrentStatus = currentStatus;
        AttemptedStatus = attemptedStatus;
    }
}

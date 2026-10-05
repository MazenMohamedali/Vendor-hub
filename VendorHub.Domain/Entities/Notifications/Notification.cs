namespace VendorHub.Domain.Entities.Notifications;

using System;
using VendorHub.Domain.Common;
using VendorHub.Domain.Enums;
using VendorHub.Domain.Exceptions;

public class Notification : BaseEntity
{
    public Guid UserId { get; private set; }
    public NotificationType Type { get; private set; }
    public string Message { get; private set; } = string.Empty;
    public bool IsRead { get; private set; }
    public DateTime CreatedAtUtc { get; private set; } = DateTime.UtcNow;
    public Guid? OrderId { get; private set; }
    public Guid? ProductId { get; private set; }

    private Notification() { }

    public static Notification Create(Guid userId, NotificationType type, string message, Guid? orderId = null, Guid? productId = null)
    {
        if (string.IsNullOrWhiteSpace(message))
            throw new DomainException("Notification message is required.");

        return new Notification
        {
            UserId = userId,
            Type = type,
            Message = message.Trim(),
            IsRead = false,
            CreatedAtUtc = DateTime.UtcNow,
            OrderId = orderId,
            ProductId = productId
        };
    }

    public void MarkAsRead() => IsRead = true;
}

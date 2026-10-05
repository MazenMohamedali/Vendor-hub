namespace VendorHub.Domain.Enums;

public enum OrderStatus
{
    Pending = 0,
    Processing = 1,
    PartiallyShipped = 2,
    Shipped = 3,
    Delivered = 4,
    Cancelled = 5
}

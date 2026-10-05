namespace VendorHub.Domain.Exceptions;

public class InsufficientStockException : DomainException
{
    public string ProductName { get; }
    public int AvailableQuantity { get; }
    public int RequestedQuantity { get; }

    public InsufficientStockException(string productName, int availableQuantity, int requestedQuantity)
        : base($"Insufficient stock for product '{productName}'. Available: {availableQuantity}, Requested: {requestedQuantity}.", "INSUFFICIENT_STOCK")
    {
        ProductName = productName;
        AvailableQuantity = availableQuantity;
        RequestedQuantity = requestedQuantity;
    }
}

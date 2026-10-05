namespace VendorHub.Application.Features.Orders.CreateOrder;

public sealed record CreateOrderItemRequest(
    Guid ProductId,
    Guid VendorId,
    decimal UnitPrice,
    string Currency,
    int Quantity);

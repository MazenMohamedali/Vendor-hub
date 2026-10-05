namespace VendorHub.Application.Features.Orders.GetOrderById;

using System;
using System.Collections.Generic;

public sealed record OrderItemResponse(
    Guid Id,
    Guid ProductId,
    Guid VendorId,
    decimal UnitPrice,
    string Currency,
    int Quantity,
    string Status);

public sealed record OrderResponse(
    Guid Id,
    Guid CustomerId,
    string PhoneNumber,
    string DeliveryAddress,
    string Status,
    decimal TotalPrice,
    string Currency,
    List<OrderItemResponse> Items);

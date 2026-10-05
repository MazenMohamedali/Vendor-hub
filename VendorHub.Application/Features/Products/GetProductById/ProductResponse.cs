namespace VendorHub.Application.Features.Products.GetProductById;

using System;

public sealed record ProductResponse(
    Guid Id,
    string Name,
    decimal Price,
    string Currency,
    string ImgUrl,
    int Quantity,
    string Status,
    Guid VendorId,
    Guid CategoryId,
    long ViewersCount);

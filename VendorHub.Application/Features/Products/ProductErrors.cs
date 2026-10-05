namespace VendorHub.Application.Features.Products;

using System;
using VendorHub.Application.Common.Models;

public static class ProductErrors
{
    public static Error NotFound(Guid id) => new(
        "Product.NotFound",
        $"The product with ID '{id}' was not found.");

    public static readonly Error InsufficientStock = new(
        "Product.InsufficientStock",
        "The requested quantity exceeds available stock.");
}

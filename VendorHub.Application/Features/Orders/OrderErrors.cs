namespace VendorHub.Application.Features.Orders;

using System;
using VendorHub.Application.Common.Models;

public static class OrderErrors
{
    public static Error NotFound(Guid id) => new(
        "Order.NotFound",
        $"The order with ID '{id}' was not found.");

    public static readonly Error CannotCancelDelivered = new(
        "Order.CannotCancelDelivered",
        "Delivered orders cannot be cancelled.");

    public static readonly Error EmptyCart = new(
        "Order.EmptyCart",
        "An order must contain at least one item.");
}

namespace VendorHub.Application.Features.Orders.GetOrderById;

using System;
using VendorHub.Application.Common.Messaging;

public sealed record GetOrderByIdQuery(Guid OrderId) : IQuery<OrderResponse>;

namespace VendorHub.Application.Features.Orders.CancelOrder;

using System;
using VendorHub.Application.Common.Messaging;

public sealed record CancelOrderCommand(Guid OrderId) : ICommand;

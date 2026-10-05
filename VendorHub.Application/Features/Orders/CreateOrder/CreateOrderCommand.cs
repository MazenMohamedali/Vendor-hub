namespace VendorHub.Application.Features.Orders.CreateOrder;

using System;
using System.Collections.Generic;
using VendorHub.Application.Common.Messaging;

public sealed record CreateOrderCommand(
    Guid CustomerId,
    string Street,
    string City,
    string State,
    string Country,
    string PostalCode,
    string PhoneNumber,
    string Currency,
    List<CreateOrderItemRequest> Items) : ICommand<Guid>;
